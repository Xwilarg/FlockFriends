using TouhouJam.SO;
using Unity.Entities;
using UnityEngine;

namespace TouhouJam.Player
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public PlayerInfo Info;
    }

    public class PlayerBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Component.Player
            {
                Info = authoring.Info,
            });
        }
    }
}
