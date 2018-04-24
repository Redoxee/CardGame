using System.Collections;
using System.Collections.Generic;

namespace AMG.Framework
{
    public class Entity
    {
        private HashSet<TagEnum> m_enumTags = new HashSet<TagEnum>();
        private Dictionary<TagEnum, int> m_iTagVars = new Dictionary<TagEnum, int>();
        //private Dictionary<TagEnum, object> m_oTagVars = new Dictionary<TagEnum, object>();

        public void AddTag(TagEnum tag)
        {
            m_enumTags.Add(tag);

            EntityManager.Instance.NotifyAddTag(this, tag);
        }

        public void RemoveTag(TagEnum tag)
        {
            m_enumTags.Remove(tag);
            EntityManager.Instance.NotifyRemoveTag(this, tag);
        }

        ~Entity()
        {
            EntityManager manager = EntityManager.Instance;
            foreach (TagEnum tag in m_enumTags)
            {
                manager.NotifyRemoveTag(this, tag);
            }
        }
        
        public int GetITagValue(TagEnum tag)
        {
            if (m_iTagVars.ContainsKey(tag))
                return m_iTagVars[tag];
            return int.MaxValue;
        }

        public void SetITagValue(TagEnum tag, int value)
        {
            m_iTagVars[tag] = value;
        }

        public static HashSet<Entity> GetTagged(TagEnum tag)
        {
            return EntityManager.Instance.GetTagged(tag);
        }

        #region Manager

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

            private Dictionary<TagEnum, HashSet<Entity>> m_tagToEntity = new Dictionary<TagEnum, HashSet<Entity>>();

            public void NotifyAddTag(Entity e, TagEnum tag)
            {
                if (!m_tagToEntity.ContainsKey(tag))
                    m_tagToEntity[tag] = new HashSet<Entity>();
                m_tagToEntity[tag].Add(e);
            }

            public void NotifyRemoveTag(Entity e, TagEnum tag)
            {
                m_tagToEntity[tag].Remove(e);
            }

            public HashSet<Entity> GetTagged(TagEnum tag)
            {
                if (!m_tagToEntity.ContainsKey(tag))
                    m_tagToEntity[tag] = new HashSet<Entity>();
                return m_tagToEntity[tag];
            }
        }
        #endregion
    }
}