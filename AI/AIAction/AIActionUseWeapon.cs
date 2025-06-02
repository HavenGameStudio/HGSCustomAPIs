using HGS.LastBastion.Character.CharacterAbilities;
using UnityEngine;

namespace HGS.AI
{
    [AddComponentMenu("Haven Game Studio/AI/AI Actions/AI Use Weapon")]
    public class AIActionUseWeapon : AIAction
    {
        CharacterHandleWeapon characterHandleWeapon;
        public override void PerformAction()
        {
            characterHandleWeapon.UseWeapon();
        }

        public override void Initialization()
        {
            base.Initialization();
            characterHandleWeapon = _brain.Owner.GetComponentInChildren<CharacterHandleWeapon>();
        }
    }
}
