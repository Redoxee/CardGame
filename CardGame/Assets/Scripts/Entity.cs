using System.Collections;
using System.Collections.Generic;

namespace AMG.Framework
{
    public class Entity
    {
        private HashSet<string> m_sTags = new HashSet<string>();
        public void AddTag(string tag)
        {
            m_sTags.Add(tag);

            EntityManager.Instance.NotifyAddTag(this, tag);
        }

        public void RemoveTag(string tag)
        {
            m_sTags.Remove(tag);
            EntityManager.Instance.NotifyRemoveTag(this, tag);
        }

        ~Entity()
        {
            EntityManager manager = EntityManager.Instance;
            foreach (string tag in m_sTags)
            {
                manager.NotifyRemoveTag(this, tag);
            }
        }

        public class EntityCollection : HashSet<Entity> { }
        private class EntityManager
        {
            private static EntityManager s_instance;
            public static EntityManager Instance
            {
                get
                {
                    if (s_instance == null)
                        s_instance = new EntityManager();
                    return s_instance;
                }
            }

            private Dictionary<string, EntityCollection> m_tagToEntity = new Dictionary<string, EntityCollection>();

            public void NotifyAddTag(Entity e, string tag)
            {
                if (!m_tagToEntity.ContainsKey(tag))
                    m_tagToEntity[tag] = new EntityCollection();
                m_tagToEntity[tag].Add(e);
            }

            public void NotifyRemoveTag(Entity e, string tag)
            {
                m_tagToEntity[tag].Remove(e);
            }

            public EntityCollection GetTagged(string tag)
            {
                if (!m_tagToEntity.ContainsKey(tag))
                    m_tagToEntity[tag] = new EntityCollection();
                return m_tagToEntity[tag];
            }
        }

        public static EntityCollection GetTagged(string tag)
        {
            return EntityManager.Instance.GetTagged(tag);
        }
    }
}