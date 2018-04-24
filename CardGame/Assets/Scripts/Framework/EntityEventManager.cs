using System;
using System.Collections;
using System.Collections.Generic;

namespace AMG.Framework
{
    public abstract class Verb {
        public abstract void Apply();
    };

    public class SetIntVerb : Verb
    {
        public TagEnum target;
        public int value;

        public override void Apply()
        {
            var targets = Entity.GetTagged(target);
            foreach (var e in targets)
            {
                e.SetITagValue(target, value);
            }
        }
    }

    public class AddIntVerb : Verb
    {
        public TagEnum target;
        public int delta;

        public override void Apply()
        {
            var targets = Entity.GetTagged(target);
            foreach (var e in targets)
            {
                e.SetITagValue(target, e.GetITagValue(target) + delta);
            }
        }
    }


    public class AddTagVerb : Verb
    {
        public TagEnum tagTarget;
        public TagEnum tagAdded;
        public override void Apply()
        {
            var targets = Entity.GetTagged(tagTarget);
            foreach (var e in targets)
                e.AddTag(tagAdded);
        }
    }

    public class RemoveTagVerb : Verb
    {
        public TagEnum tagTarget;
        public TagEnum tagRemoved;
        public override void Apply()
        {
            var targets = Entity.GetTagged(tagTarget);
            foreach (var e in targets)
                e.RemoveTag(tagRemoved);
        }
    }
}