using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Globalization;

namespace CxViewerAction.Entities
{
    /// <summary>
    /// Entity data
    /// </summary>
    public struct EntityId : IEquatable<EntityId>, IComparable<EntityId>, IComparable, IConvertible
    {
        object _Id;

        public EntityId(object id)
        {
            if (id is DBNull)
            {
                this._Id = null;
                return;
            }
            if (id is EntityId)
            {
                this._Id = ((EntityId)id)._Id;
                return;
            }

            this._Id = id;
        }

        public EntityId(EntityId id)
        {
            this._Id = id._Id;
        }

        public object Id
        {
            get
            {
                return this._Id;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this._Id == null || this._Id == DBNull.Value;
            }
        }

        public object DbValue
        {
            get
            {
                if (this.IsEmpty)
                    return DBNull.Value;
                else
                    return this._Id;
            }
        }

        public T GetId<T>(T nullValue)
        {
            if (this.IsEmpty)
                return nullValue;
            else
                return (T)this._Id;
        }

        public override bool Equals(object right)
        {
            return this.Equals(new EntityId(right));
        }

        public bool Equals(EntityId id)
        {
            if (object.ReferenceEquals(this, id))
                return true;

            if (this.IsEmpty && id.IsEmpty)
                return true;

            if (this._Id == id._Id)
                return true;

            if (this._Id != null && id._Id != null && this._Id.GetType() != id._Id.GetType())
                return false;

            return ((IComparable<EntityId>)this).CompareTo(id) == 0;
        }

        public override int GetHashCode()
        {
            if (!this.IsEmpty)
                return this._Id.GetHashCode();

            return base.GetHashCode();
        }

        public override string ToString()
        {
            if (this.IsEmpty)
                return string.Empty;
            else
                return Convert.ToString(this._Id.ToString(), CultureInfo.InvariantCulture);
        }

        static public bool operator ==(EntityId left, EntityId right)
        {
            return left.Equals(right);
        }

        static public bool operator ==(object left, EntityId right)
        {
            return right.Equals(left);
        }

        static public bool operator ==(EntityId left, object right)
        {
            return left.Equals(right);
        }

        static public bool operator !=(EntityId left, EntityId right)
        {
            return !left.Equals(right);
        }

        static public bool operator !=(object left, EntityId right)
        {
            return !right.Equals(left);
        }

        static public bool operator !=(EntityId left, object right)
        {
            return !left.Equals(right);
        }

        static public bool operator >(EntityId left, EntityId right)
        {
            return (left as IComparable<EntityId>).CompareTo(right) > 0;
        }

        static public bool operator <(EntityId left, EntityId right)
        {
            return (left as IComparable<EntityId>).CompareTo(right) < 0;
        }

        static public bool operator >=(EntityId left, EntityId right)
        {
            return left > right || left == right;
        }

        static public bool operator <=(EntityId left, EntityId right)
        {
            return left < right || left == right;
        }

        static public EntityId Empty
        {
            get
            {
                return new EntityId();
            }
        }

        static public implicit operator EntityId(int intId)
        {
            return new EntityId((object)intId);
        }

        static public explicit operator int(EntityId entityId)
        {
            return (int)entityId._Id;
        }

        static public string JoinIDs(EntityId[] aIDs)
        {
            StringBuilder lBuilder = new StringBuilder();
            foreach (EntityId lID in aIDs)
            {
                if (lID.IsEmpty)
                    continue;

                if (lBuilder.Length > 0)
                {
                    lBuilder.Append(",");
                }

                lBuilder.Append(lID.ToString());
            }
            return lBuilder.ToString();
        }

        #region IEquatable<EntityID> Members

        bool IEquatable<EntityId>.Equals(EntityId id)
        {
            return this.Equals(id);
        }

        #endregion

        #region IComparable<EntityID> Members

        int IComparable<EntityId>.CompareTo(EntityId cmpTo)
        {
            if (this._Id == cmpTo._Id)
                return 0;

            if (this.IsEmpty && !cmpTo.IsEmpty)
                return -1;

            if (!this.IsEmpty && cmpTo.IsEmpty)
                return 1;



            if (this._Id is IComparable)
                return ((IComparable)this._Id).CompareTo(cmpTo._Id);
            else if (cmpTo._Id is IComparable)
                return -((IComparable)cmpTo._Id).CompareTo(this._Id);

            throw new ArgumentException(
                String.Format("Can't compare EntityID of type {0} with EntityID of type {1}",
                    this._Id.GetType().ToString(),
                    cmpTo._Id.GetType().ToString()));

        }

        #endregion

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            return (this as IComparable<EntityId>).CompareTo(new EntityId(obj));

        }

        #endregion

        #region IConvertible Members

        TypeCode IConvertible.GetTypeCode()
        {
            if (this.IsEmpty)
                return TypeCode.Object;

            IConvertible lConv;
            lConv = this._Id as IConvertible;
            if (lConv == null)
                return TypeCode.Object;
            else return lConv.GetTypeCode();
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            if (this.IsEmpty)
                return false;
            if (this > 0)
                return true;
            return false;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            if (this.IsEmpty) return -1;

            IConvertible lConv;
            lConv = this._Id as IConvertible;
            if (lConv == null)
                return (-1 as IConvertible).ToInt32(provider);
            else return lConv.ToInt32(provider);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return this.ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}