﻿using System;

namespace AlphaChiTech.Virtualization.Actions
{
    public class PlaceholderReplaceWa<T> : BaseActionVirtualization where T : class
    {
        private readonly T _oldValue;
        private readonly T _newValue;
        private readonly int _index;

        readonly WeakReference _voc;

        public PlaceholderReplaceWa(VirtualizingObservableCollection<T> voc, T oldValue, T newValue, int index)
            : base(VirtualActionThreadModelEnum.UseUiThread)
        {
            this._voc = new WeakReference(voc);
            this._oldValue = oldValue;
            this._newValue = newValue;
            this._index = index;
        }

        public override void DoAction()
        {
            var voc = (VirtualizingObservableCollection<T>)this._voc.Target;

            if (voc != null && this._voc.IsAlive)
            {
                voc.ReplaceAt(this._index, this._oldValue, this._newValue, null);
            }
        }
    }

}
