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
  public partial class MNMessagesSyncDeltaMarkRead : TBase
  {
    private List<MNMessagesSyncThreadKey> _ThreadKeys;
    private List<int> _Folders;
    private long _WatermarkTimestamp;
    private long _ActionTimestamp;

    public List<MNMessagesSyncThreadKey> ThreadKeys
    {
      get
      {
        return _ThreadKeys;
      }
      set
      {
        __isset.ThreadKeys = true;
        this._ThreadKeys = value;
      }
    }

    public List<int> Folders
    {
      get
      {
        return _Folders;
      }
      set
      {
        __isset.Folders = true;
        this._Folders = value;
      }
    }

    public long WatermarkTimestamp
    {
      get
      {
        return _WatermarkTimestamp;
      }
      set
      {
        __isset.WatermarkTimestamp = true;
        this._WatermarkTimestamp = value;
      }
    }

    public long ActionTimestamp
    {
      get
      {
        return _ActionTimestamp;
      }
      set
      {
        __isset.ActionTimestamp = true;
        this._ActionTimestamp = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool ThreadKeys;
      public bool Folders;
      public bool WatermarkTimestamp;
      public bool ActionTimestamp;
    }

    public MNMessagesSyncDeltaMarkRead() {
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
              if (field.Type == TType.List) {
                {
                  ThreadKeys = new List<MNMessagesSyncThreadKey>();
                  TList _list39 = iprot.ReadListBegin();
                  for( int _i40 = 0; _i40 < _list39.Count; ++_i40)
                  {
                    MNMessagesSyncThreadKey _elem41;
                    _elem41 = new MNMessagesSyncThreadKey();
                    _elem41.Read(iprot);
                    ThreadKeys.Add(_elem41);
                  }
                  iprot.ReadListEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.List) {
                {
                  Folders = new List<int>();
                  TList _list42 = iprot.ReadListBegin();
                  for( int _i43 = 0; _i43 < _list42.Count; ++_i43)
                  {
                    int _elem44;
                    _elem44 = iprot.ReadI32();
                    Folders.Add(_elem44);
                  }
                  iprot.ReadListEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.I64) {
                WatermarkTimestamp = iprot.ReadI64();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.I64) {
                ActionTimestamp = iprot.ReadI64();
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
        TStruct struc = new TStruct("MNMessagesSyncDeltaMarkRead");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (ThreadKeys != null && __isset.ThreadKeys) {
          field.Name = "ThreadKeys";
          field.Type = TType.List;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.Struct, ThreadKeys.Count));
            foreach (MNMessagesSyncThreadKey _iter45 in ThreadKeys)
            {
              _iter45.Write(oprot);
            }
            oprot.WriteListEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (Folders != null && __isset.Folders) {
          field.Name = "Folders";
          field.Type = TType.List;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.I32, Folders.Count));
            foreach (int _iter46 in Folders)
            {
              oprot.WriteI32(_iter46);
            }
            oprot.WriteListEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (__isset.WatermarkTimestamp) {
          field.Name = "WatermarkTimestamp";
          field.Type = TType.I64;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteI64(WatermarkTimestamp);
          oprot.WriteFieldEnd();
        }
        if (__isset.ActionTimestamp) {
          field.Name = "ActionTimestamp";
          field.Type = TType.I64;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteI64(ActionTimestamp);
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
      StringBuilder __sb = new StringBuilder("MNMessagesSyncDeltaMarkRead(");
      bool __first = true;
      if (ThreadKeys != null && __isset.ThreadKeys) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ThreadKeys: ");
        __sb.Append(ThreadKeys);
      }
      if (Folders != null && __isset.Folders) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Folders: ");
        __sb.Append(Folders);
      }
      if (__isset.WatermarkTimestamp) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("WatermarkTimestamp: ");
        __sb.Append(WatermarkTimestamp);
      }
      if (__isset.ActionTimestamp) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ActionTimestamp: ");
        __sb.Append(ActionTimestamp);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
