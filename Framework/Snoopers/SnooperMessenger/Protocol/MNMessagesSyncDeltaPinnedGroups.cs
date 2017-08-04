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
  public partial class MNMessagesSyncDeltaPinnedGroups : TBase
  {
    private List<MNMessagesSyncThreadKey> _ThreadKeys;

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


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool ThreadKeys;
    }

    public MNMessagesSyncDeltaPinnedGroups() {
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
                  TList _list103 = iprot.ReadListBegin();
                  for( int _i104 = 0; _i104 < _list103.Count; ++_i104)
                  {
                    MNMessagesSyncThreadKey _elem105;
                    _elem105 = new MNMessagesSyncThreadKey();
                    _elem105.Read(iprot);
                    ThreadKeys.Add(_elem105);
                  }
                  iprot.ReadListEnd();
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
        TStruct struc = new TStruct("MNMessagesSyncDeltaPinnedGroups");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (ThreadKeys != null && __isset.ThreadKeys) {
          field.Name = "ThreadKeys";
          field.Type = TType.List;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.Struct, ThreadKeys.Count));
            foreach (MNMessagesSyncThreadKey _iter106 in ThreadKeys)
            {
              _iter106.Write(oprot);
            }
            oprot.WriteListEnd();
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
      StringBuilder __sb = new StringBuilder("MNMessagesSyncDeltaPinnedGroups(");
      bool __first = true;
      if (ThreadKeys != null && __isset.ThreadKeys) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ThreadKeys: ");
        __sb.Append(ThreadKeys);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
