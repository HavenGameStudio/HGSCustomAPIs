﻿//#define EVENTROUTER_THROWEXCEPTIONS 
#if EVENTROUTER_THROWEXCEPTIONS
//#define EVENTROUTER_REQUIRELISTENER // Uncomment this if you want listeners to be required for sending events.
#endif

using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace HGS.Tools
{
	/// <summary>
	/// GameEvents are used throughout the game for general game events (game started, game ended, life lost, etc.)
	/// </summary>
	public struct HGSGameEvent
	{
		static HGSGameEvent e;

		public string EventName;
		public int IntParameter;
		public Vector2 Vector2Parameter;
		public Vector3 Vector3Parameter;
		public bool BoolParameter;
		public string StringParameter;

		public static void Trigger(string eventName, int intParameter = 0, Vector2 vector2Parameter = default(Vector2), Vector3 vector3Parameter = default(Vector3), bool boolParameter = false, string stringParameter = "")
		{
			e.EventName = eventName;
			e.IntParameter = intParameter;
			e.Vector2Parameter = vector2Parameter;
			e.Vector3Parameter = vector3Parameter;
			e.BoolParameter = boolParameter;
			e.StringParameter = stringParameter;
			HGSEventManager.TriggerEvent(e);
		}
	}

	/// <summary>
	/// This class handles event management, and can be used to broadcast events throughout the game, to tell one class (or many) that something's happened.
	/// Events are structs, you can define any kind of events you want. This manager comes with GameEvents, which are 
	/// basically just made of a string, but you can work with more complex ones if you want.
	/// 
	/// To trigger a new event, from anywhere, do YOUR_EVENT.Trigger(YOUR_PARAMETERS)
	/// So GameEvent.Trigger("Save"); for example will trigger a Save GameEvent
	/// 
	/// you can also call EventManager.TriggerEvent(YOUR_EVENT);
	/// For example : EventManager.TriggerEvent(new GameEvent("GameStart")); will broadcast an GameEvent named GameStart to all listeners.
	///
	/// To start listening to an event from any class, there are 3 things you must do : 
	///
	/// 1 - tell that your class implements the EventListener interface for that kind of event.
	/// For example: public class GUIManager : Singleton<GUIManager>, EventListener<GameEvent>
	/// You can have more than one of these (one per event type).
	///
	/// 2 - On Enable and Disable, respectively start and stop listening to the event :
	/// void OnEnable()
	/// {
	/// 	this.EventStartListening<GameEvent>();
	/// }
	/// void OnDisable()
	/// {
	/// 	this.EventStopListening<GameEvent>();
	/// }
	/// 
	/// 3 - Implement the EventListener interface for that event. For example :
	/// public void OnEvent(GameEvent gameEvent)
	/// {
	/// 	if (gameEvent.EventName == "GameOver")
	///		{
	///			// DO SOMETHING
	///		}
	/// } 
	/// will catch all events of type GameEvent emitted from anywhere in the game, and do something if it's named GameOver
	/// </summary>
	[ExecuteAlways]
	public static class HGSEventManager
	{
		private static Dictionary<Type, List<HGSEventListenerBase>> _subscribersList;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void InitializeStatics()
		{
			_subscribersList = new Dictionary<Type, List<HGSEventListenerBase>>();
		}

		static HGSEventManager()
		{
			_subscribersList = new Dictionary<Type, List<HGSEventListenerBase>>();
		}

		/// <summary>
		/// Adds a new subscriber to a certain event.
		/// </summary>
		/// <param name="listener">listener.</param>
		/// <typeparam name="HGSEvent">The event type.</typeparam>
		public static void AddListener<HGSEvent>(HGSEventListener<HGSEvent> listener) where HGSEvent : struct
		{
			Type eventType = typeof(HGSEvent);

			if (!_subscribersList.ContainsKey(eventType))
			{
				_subscribersList[eventType] = new List<HGSEventListenerBase>();
			}

			if (!SubscriptionExists(eventType, listener))
			{
				_subscribersList[eventType].Add(listener);
			}
		}

		/// <summary>
		/// Removes a subscriber from a certain event.
		/// </summary>
		/// <param name="listener">listener.</param>
		/// <typeparam name="HGSEvent">The event type.</typeparam>
		public static void RemoveListener<HGSEvent>(HGSEventListener<HGSEvent> listener) where HGSEvent : struct
		{
			Type eventType = typeof(HGSEvent);

			if (!_subscribersList.ContainsKey(eventType))
			{
#if EVENTROUTER_THROWEXCEPTIONS
					throw new ArgumentException( string.Format( "Removing listener \"{0}\", but the event type \"{1}\" isn't registered.", listener, eventType.ToString() ) );
#else
				return;
#endif
			}

			List<HGSEventListenerBase> subscriberList = _subscribersList[eventType];

#if EVENTROUTER_THROWEXCEPTIONS
	            bool listenerFound = false;
#endif

			for (int i = subscriberList.Count - 1; i >= 0; i--)
			{
				if (subscriberList[i] == listener)
				{
					subscriberList.Remove(subscriberList[i]);
#if EVENTROUTER_THROWEXCEPTIONS
					    listenerFound = true;
#endif

					if (subscriberList.Count == 0)
					{
						_subscribersList.Remove(eventType);
					}

					return;
				}
			}

#if EVENTROUTER_THROWEXCEPTIONS
		        if( !listenerFound )
		        {
					throw new ArgumentException( string.Format( "Removing listener, but the supplied receiver isn't subscribed to event type \"{0}\".", eventType.ToString() ) );
		        }
#endif
		}

		/// <summary>
		/// Triggers an event. All instances that are subscribed to it will receive it (and will potentially act on it).
		/// </summary>
		/// <param name="newEvent">The event to trigger.</param>
		/// <typeparam name="HGSEvent">The 1st type parameter.</typeparam>
		public static void TriggerEvent<HGSEvent>(HGSEvent newEvent) where HGSEvent : struct
		{
			List<HGSEventListenerBase> list;
			if (!_subscribersList.TryGetValue(typeof(HGSEvent), out list))
#if EVENTROUTER_REQUIRELISTENER
			            throw new ArgumentException( string.Format( "Attempting to send event of type \"{0}\", but no listener for this type has been found. Make sure this.Subscribe<{0}>(EventRouter) has been called, or that all listeners to this event haven't been unsubscribed.", typeof( HGSEvent ).ToString() ) );
#else
				return;
#endif

			for (int i = list.Count - 1; i >= 0; i--)
			{
				(list[i] as HGSEventListener<HGSEvent>).OnHGSEvent(newEvent);
			}
		}

		/// <summary>
		/// Checks if there are subscribers for a certain type of events
		/// </summary>
		/// <returns><c>true</c>, if exists was subscriptioned, <c>false</c> otherwise.</returns>
		/// <param name="type">Type.</param>
		/// <param name="receiver">Receiver.</param>
		private static bool SubscriptionExists(Type type, HGSEventListenerBase receiver)
		{
			List<HGSEventListenerBase> receivers;

			if (!_subscribersList.TryGetValue(type, out receivers)) return false;

			bool exists = false;

			for (int i = receivers.Count - 1; i >= 0; i--)
			{
				if (receivers[i] == receiver)
				{
					exists = true;
					break;
				}
			}

			return exists;
		}
	}

	/// <summary>
	/// Static class that allows any class to start or stop listening to events
	/// </summary>
	public static class EventRegister
	{
		public delegate void Delegate<T>(T eventType);

		public static void HGSEventStartListening<EventType>(this HGSEventListener<EventType> caller) where EventType : struct
		{
			HGSEventManager.AddListener<EventType>(caller);
		}

		public static void HGSEventStopListening<EventType>(this HGSEventListener<EventType> caller) where EventType : struct
		{
			HGSEventManager.RemoveListener<EventType>(caller);
		}
	}

	/// <summary>
	/// Event listener basic interface
	/// </summary>
	public interface HGSEventListenerBase { };

	/// <summary>
	/// A public interface you'll need to implement for each type of event you want to listen to.
	/// </summary>
	public interface HGSEventListener<T> : HGSEventListenerBase
	{
		void OnHGSEvent(T eventType);
	}

	public class HGSEventListenerWrapper<TOwner, TTarget, TEvent> : HGSEventListener<TEvent>, IDisposable
		where TEvent : struct
	{
		private Action<TTarget> _callback;

		private TOwner _owner;
		public HGSEventListenerWrapper(TOwner owner, Action<TTarget> callback)
		{
			_owner = owner;
			_callback = callback;
			RegisterCallbacks(true);
		}

		public void Dispose()
		{
			RegisterCallbacks(false);
			_callback = null;
		}

		protected virtual TTarget OnEvent(TEvent eventType) => default;
		public void OnHGSEvent(TEvent eventType)
		{
			var item = OnEvent(eventType);
			_callback?.Invoke(item);
		}

		private void RegisterCallbacks(bool b)
		{
			if (b)
			{
				this.HGSEventStartListening<TEvent>();
			}
			else
			{
				this.HGSEventStopListening<TEvent>();
			}
		}
	}
}