// Copyright (c) 2017 Jan Pluskal, Viliam Letavay
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

/**
 * Autogenerated by Thrift Compiler (0.9.3)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Netfox.SnooperMessenger.Protocol
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class MNMessagesSyncDeltaFolderCount : TBase
  {
    private int _ThreadFolder;
    private int _Count;
    private bool _HasMore;
    private Dictionary<int, MNMessagesSyncTagCount> _Counts;

    public int ThreadFolder
    {
      get
      {
        return _ThreadFolder;
      }
      set
      {
        __isset.ThreadFolder = true;
        this._ThreadFolder = value;
      }
    }

    public int Count
    {
      get
      {
        return _Count;
      }
      set
      {
        __isset.Count = true;
        this._Count = value;
      }
    }

    public bool HasMore
    {
      get
      {
        return _HasMore;
      }
      set
      {
        __isset.HasMore = true;
        this._HasMore = value;
      }
    }

    public Dictionary<int, MNMessagesSyncTagCount> Counts
    {
      get
      {
        return _Counts;
      }
      set
      {
        __isset.Counts = true;
        this._Counts = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool ThreadFolder;
      public bool Count;
      public bool HasMore;
      public bool Counts;
    }

    public MNMessagesSyncDeltaFolderCount() {
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.I32) {
                ThreadFolder = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.I32) {
                Count = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.Bool) {
                HasMore = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.Map) {
                {
                  Counts = new Dictionary<int, MNMessagesSyncTagCount>();
                  TMap _map111 = iprot.ReadMapBegin();
                  for( int _i112 = 0; _i112 < _map111.Count; ++_i112)
                  {
                    int _key113;
                    MNMessagesSyncTagCount _val114;
                    _key113 = iprot.ReadI32();
                    _val114 = new MNMessagesSyncTagCount();
                    _val114.Read(iprot);
                    Counts[_key113] = _val114;
                  }
                  iprot.ReadMapEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public void Write(TProtocol oprot) {
      oprot.IncrementRecursionDepth();
      try
      {
        TStruct struc = new TStruct("MNMessagesSyncDeltaFolderCount");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (__isset.ThreadFolder) {
          field.Name = "ThreadFolder";
          field.Type = TType.I32;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(ThreadFolder);
          oprot.WriteFieldEnd();
        }
        if (__isset.Count) {
          field.Name = "Count";
          field.Type = TType.I32;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(Count);
          oprot.WriteFieldEnd();
        }
        if (__isset.HasMore) {
          field.Name = "HasMore";
          field.Type = TType.Bool;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(HasMore);
          oprot.WriteFieldEnd();
        }
        if (Counts != null && __isset.Counts) {
          field.Name = "Counts";
          field.Type = TType.Map;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteMapBegin(new TMap(TType.I32, TType.Struct, Counts.Count));
            foreach (int _iter115 in Counts.Keys)
            {
              oprot.WriteI32(_iter115);
              Counts[_iter115].Write(oprot);
            }
            oprot.WriteMapEnd();
          }
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("MNMessagesSyncDeltaFolderCount(");
      bool __first = true;
      if (__isset.ThreadFolder) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ThreadFolder: ");
        __sb.Append(ThreadFolder);
      }
      if (__isset.Count) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Count: ");
        __sb.Append(Count);
      }
      if (__isset.HasMore) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("HasMore: ");
        __sb.Append(HasMore);
      }
      if (Counts != null && __isset.Counts) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Counts: ");
        __sb.Append(Counts);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
