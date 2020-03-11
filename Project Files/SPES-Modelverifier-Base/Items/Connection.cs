﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SPES_Modelverifier_Base.Items
{
    public abstract class Connection : BaseObject
    {
        /// <summary>
        /// a connection item has to define what it can be connected to. Empty list means it can connect any object derived from BaseObject
        /// needs EXPLICIT XmlIgnore in derived class!
        /// </summary>
        [XmlIgnore]
        public abstract List<Type> AllowedConnectedTypes { get; }

        /// <summary>
        /// define if the connection needs to be inverted (in case shape is directional and aligned wrong).
        /// </summary>
        public abstract bool Inverted { get; }

        /// <summary>
        /// the object the connector points to
        /// </summary>
        public BaseObject FromObject { get; set; }

        /// <summary>
        /// the object the connector points from
        /// </summary>
        public BaseObject ToObject { get; set; }

        /// <summary>
        /// defines if the connection item only connects with one end
        /// </summary>
        public virtual bool AllowOnlyOneConnectedItem => false;

        /// <summary>
        /// searches all items in a model and sets the pointer if a match has been found
        /// </summary>
        /// <param name="pAllObjects"></param>
        public void SetConnections(List<BaseObject> pAllObjects)
        {
            //set from and to functions
            BaseObject xFromObject = this.GetObjectConnectingFrom(pAllObjects);
            BaseObject xToObject = this.GetObjectConnectingTo(pAllObjects);

            //differentiate on allowonlyoneconnecteditem
            if (AllowOnlyOneConnectedItem)
            {
                if (xFromObject != null && (AllowedConnectedTypes.Any() ? AllowedConnectedTypes.Contains(xFromObject.GetType()) : true))
                {
                    SetConnectionValue(null, xFromObject);
                }
                else if (xToObject != null && (AllowedConnectedTypes.Any() ? AllowedConnectedTypes.Contains(xToObject.GetType()) : true))
                {
                    SetConnectionValue(xToObject, null);
                }
                else
                {
                    throw new ValidationFailedException(this, this.GetType().Name + " " + this.Uniquename + " doesn't connect to an allowed types and/or connected item is null.");
                }
            }
            else
            {
                if (xFromObject != null && xToObject != null && 
                    (AllowedConnectedTypes.Any() ? AllowedConnectedTypes.Contains(xFromObject.GetType()) : true) &&
                    (AllowedConnectedTypes.Any() ? AllowedConnectedTypes.Contains(xToObject.GetType()) : true))
                {
                    SetConnectionValue(xToObject,xFromObject);
                }
                else
                {
                    throw new ValidationFailedException(this, this.GetType().Name + " " + this.Uniquename + " doesn't connect to two allowed types and/or connected items are null.");
                }
            }
        }

        private BaseObject GetObjectConnectingTo(List<BaseObject> pAllObjects)
        {
            try
            {
                return pAllObjects.Find(t => t.Visioshape == this.Visioshape.Connects[1].ToSheet);
            }
            catch { return null; }
        }

        private BaseObject GetObjectConnectingFrom(List<BaseObject> pAllObjects)
        {
            try
            {
                return pAllObjects.Find(t => t.Visioshape == this.Visioshape.Connects[2].ToSheet);
            }
            catch { return null; }
        }

        private void SetConnectionValue(BaseObject pToObject, BaseObject pFromObject)
        {
            //flip from and to because visio
            if (Inverted)
            {
                this.FromObject = pToObject;
                this.ToObject = pFromObject;
            }
            else
            {
                this.FromObject = pFromObject;
                this.ToObject = pToObject;
            }

            //set connection items at target objects
            (FromObject as Item)?.Connections.Add(this);
            (ToObject as Item)?.Connections.Add(this);
        }
    }
}
