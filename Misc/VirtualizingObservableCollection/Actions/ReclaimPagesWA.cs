﻿using System;
using AlphaChiTech.Virtualization.Interfaces;

namespace AlphaChiTech.Virtualization.Actions
{
    public class ReclaimPagesWa : BaseRepeatableActionVirtualization
    {
        public ReclaimPagesWa(IReclaimableService provider, string sectionContext)
            : base(VirtualActionThreadModelEnum.Background, true, TimeSpan.FromMinutes(1))
        {
            this._wrProvider = new WeakReference(provider);
        }

        readonly WeakReference _wrProvider = null;

        readonly string _sectionContext = "";

        public override void DoAction()
        {
            this.LastRun = DateTime.Now;

            var reclaimer = this._wrProvider.Target as IReclaimableService;

            if (reclaimer != null)
            {
                reclaimer.RunClaim(this._sectionContext);
            }
        }

        public override bool KeepInActionsList()
        {
            var ret = base.KeepInActionsList();

            if (!this._wrProvider.IsAlive) ret = false;

            return ret;
        }
    }

}
