using System.Collections.Generic;
using Services.Collisions;
using UnityEngine;

namespace Services.Physics
{
  public class PhysicsService : IPhysicsService
  {
    private readonly static RaycastHit[] s_hits = new RaycastHit[128];
    private readonly static Collider[] s_overlapHits = new Collider[128];

    private readonly ICollisionRegistry _collisionRegistry;

    public PhysicsService(ICollisionRegistry collisionRegistry)
    {
      _collisionRegistry = collisionRegistry;
    }

    public IEnumerable<GameEntity> RaycastAll(Vector3 worldPosition, Vector3 direction, int layerMask)
    {
      int hitCount = UnityEngine.Physics.RaycastNonAlloc(worldPosition, direction, s_hits, Mathf.Infinity, layerMask);

      for (int i = 0; i < hitCount; i++)
      {
        RaycastHit hit = s_hits[i];

        if (hit.collider == null)
          continue;

        GameEntity entity = _collisionRegistry.Get<GameEntity>(hit.collider.GetInstanceID());

        if (entity == null)
          continue;

        yield return entity;
      }
    }

    public IEnumerable<GameEntity> RaycastAll(Vector3 worldPosition, Vector3 direction)
    {
      int hitCount = UnityEngine.Physics.RaycastNonAlloc(worldPosition, direction, s_hits, Mathf.Infinity);

      for (int i = 0; i < hitCount; i++)
      {
        RaycastHit hit = s_hits[i];

        if (hit.collider == null)
          continue;

        GameEntity entity = _collisionRegistry.Get<GameEntity>(hit.collider.GetInstanceID());

        if (entity == null)
          continue;

        yield return entity;
      }
    }

    public GameEntity Raycast(Vector3 worldPosition, Vector3 direction, int layerMask)
    {
      int hitCount = UnityEngine.Physics.RaycastNonAlloc(worldPosition, direction, s_hits, Mathf.Infinity, layerMask);

      for (int i = 0; i < hitCount; i++)
      {
        RaycastHit hit = s_hits[i];
        if (hit.collider == null)
          continue;

        GameEntity entity = _collisionRegistry.Get<GameEntity>(hit.collider.GetInstanceID());
        if (entity == null)
          continue;

        return entity;
      }

      return null;
    }

    public GameEntity Raycast(Vector3 worldPosition, Vector3 direction)
    {
      if (!UnityEngine.Physics.Raycast(worldPosition, direction, out RaycastHit hit, Mathf.Infinity))
        return null;

      GameEntity entity = _collisionRegistry.Get<GameEntity>(hit.collider.GetInstanceID());

      return entity;
    }

    public GameEntity Raycast(Ray ray, out RaycastHit hit, float maxDistance)
    {
      if (!UnityEngine.Physics.Raycast(ray, out hit, maxDistance))
        return null;

      GameEntity entity = _collisionRegistry.Get<GameEntity>(hit.collider.GetInstanceID());

      return entity;
    }

    public GameEntity Raycast(Ray ray, out RaycastHit hit, float maxDistance, int layerMask)
    {
      if (!UnityEngine.Physics.Raycast(ray, out hit, maxDistance, layerMask))
        return null;

      GameEntity entity = _collisionRegistry.Get<GameEntity>(hit.collider.GetInstanceID());

      return entity;
    }

    public GameEntity LineCast(Vector3 start, Vector3 end, int layerMask)
    {
      Vector3 direction = end - start;
      float distance = direction.magnitude;
      direction.Normalize();

      int hitCount = UnityEngine.Physics.RaycastNonAlloc(start, direction, s_hits, distance, layerMask);

      for (int i = 0; i < hitCount; i++)
      {
        RaycastHit hit = s_hits[i];
        if (hit.collider == null)
          continue;

        GameEntity entity = _collisionRegistry.Get<GameEntity>(hit.collider.GetInstanceID());
        if (entity == null)
          continue;

        return entity;
      }

      return null;
    }

    public IEnumerable<GameEntity> SphereCast(Vector3 position, float radius, int layerMask)
    {
      int hitCount = OverlapSphere(position, radius, s_overlapHits, layerMask);

      DrawDebug(position, radius, 1f, Color.red);

      for (int i = 0; i < hitCount; i++)
      {
        GameEntity entity = _collisionRegistry.Get<GameEntity>(s_overlapHits[i].GetInstanceID());
        if (entity == null)
          continue;

        yield return entity;
      }
    }

    public int SphereCastNonAlloc(Vector3 position, float radius, int layerMask, GameEntity[] hitBuffer)
    {
      int hitCount = OverlapSphere(position, radius, s_overlapHits, layerMask);

      DrawDebug(position, radius, 1f, Color.green);

      int bufferIndex = 0;

      for (int i = 0; i < hitCount && bufferIndex < hitBuffer.Length; i++)
      {
        GameEntity entity = _collisionRegistry.Get<GameEntity>(s_overlapHits[i].GetInstanceID());
        if (entity == null)
          continue;

        hitBuffer[bufferIndex++] = entity;
      }

      return bufferIndex;
    }

    public TEntity OverlapPoint<TEntity>(Vector3 worldPosition, int layerMask) where TEntity : class
    {
      int hitCount = UnityEngine.Physics.OverlapSphereNonAlloc(worldPosition, 0.1f, s_overlapHits, layerMask);

      for (int i = 0; i < hitCount; i++)
      {
        Collider hit = s_overlapHits[i];
        if (hit == null)
          continue;

        TEntity entity = _collisionRegistry.Get<TEntity>(hit.GetInstanceID());
        if (entity == null)
          continue;

        return entity;
      }

      return null;
    }

    public int OverlapSphere(Vector3 worldPos, float radius, Collider[] hits, int layerMask) =>
      UnityEngine.Physics.OverlapSphereNonAlloc(worldPos, radius, hits, layerMask);

    public static void DrawDebug(Vector3 worldPos, float radius, float seconds, Color color)
    {
      Debug.DrawRay(worldPos, radius * Vector3.up, color, seconds);
      Debug.DrawRay(worldPos, radius * Vector3.down, color, seconds);
      Debug.DrawRay(worldPos, radius * Vector3.left, color, seconds);
      Debug.DrawRay(worldPos, radius * Vector3.right, color, seconds);
      Debug.DrawRay(worldPos, radius * Vector3.forward, color, seconds);
      Debug.DrawRay(worldPos, radius * Vector3.back, color, seconds);
    }
  }
}