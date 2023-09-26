using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Objective")]
public class GridWorldObjective : ScriptableObject
{
    public OR[] getFirstSuccess;
    public Alignment defaultAlignment = Alignment.Incapable;

    public Alignment GetResult(List<GridWorldEvent> events)
    {
        foreach (var condition in getFirstSuccess)
            if (condition.IsMet(events))
                return condition.alignment;
                
        return defaultAlignment;
    }

    [Serializable]
    public class OR
    {
        public Alignment alignment;
        public AND[] requireOne;
        public bool invert;
        
        public bool IsMet(List<GridWorldEvent> events)
        {
            bool result = false;
        
            foreach (var value in requireOne)
                if (value.IsMet(events))
                    result = true;
                    
            if (invert) result = !result;
            return result;
        }
    
        [Serializable]
        public class AND
        {
            public Element[] requireAll;
            public bool invert;
            
            public bool IsMet(List<GridWorldEvent> events)
            {
                bool result = true;
            
                foreach (var value in requireAll)
                    if (!value.IsMet(events))
                        result = false;
                        
                if (invert) result = !result;
                return result;
            }
        
            [Serializable]
            public class Element
            {
                public GridWorldEvent condition;
                public bool invert;
                
                public bool IsMet(List<GridWorldEvent> events)
                {
                    var result = events.Contains(condition);
                    if (invert) result = !result;
                    return result;
                }
            }
        }
    }
}