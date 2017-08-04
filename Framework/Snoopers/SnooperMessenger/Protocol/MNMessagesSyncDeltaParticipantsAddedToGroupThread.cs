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
  public partial class MNMessagesSyncDeltaParticipantsAddedToGroupThread : TBase
  {
    private MNMessagesSyncMessageMetadata _MessageMetadata;
    private List<MNMessagesSyncParticipantInfo> _AddedParticipants;

    public MNMessagesSyncMessageMetadata MessageMetadata
    {
      get
      {
        return _MessageMetadata;
      }
      set
      {
        __isset.MessageMetadata = true;
        this._MessageMetadata = value;
      }
    }

    public List<MNMessagesSyncParticipantInfo> AddedParticipants
    {
      get
      {
        return _AddedParticipants;
      }
      set
      {
        __isset.AddedParticipants = true;
        this._AddedParticipants = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool MessageMetadata;
      public bool AddedParticipants;
    }

    public MNMessagesSyncDeltaParticipantsAddedToGroupThread() {
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
              if (field.Type == TType.Struct) {
                MessageMetadata = new MNMessagesSyncMessageMetadata();
                MessageMetadata.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.List) {
                {
                  AddedParticipants = new List<MNMessagesSyncParticipantInfo>();
                  TList _list63 = iprot.ReadListBegin();
                  for( int _i64 = 0; _i64 < _list63.Count; ++_i64)
                  {
                    MNMessagesSyncParticipantInfo _elem65;
                    _elem65 = new MNMessagesSyncParticipantInfo();
                    _elem65.Read(iprot);
                    AddedParticipants.Add(_elem65);
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
        TStruct struc = new TStruct("MNMessagesSyncDeltaParticipantsAddedToGroupThread");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (MessageMetadata != null && __isset.MessageMetadata) {
          field.Name = "MessageMetadata";
          field.Type = TType.Struct;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          MessageMetadata.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (AddedParticipants != null && __isset.AddedParticipants) {
          field.Name = "AddedParticipants";
          field.Type = TType.List;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.Struct, AddedParticipants.Count));
            foreach (MNMessagesSyncParticipantInfo _iter66 in AddedParticipants)
            {
              _iter66.Write(oprot);
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
      StringBuilder __sb = new StringBuilder("MNMessagesSyncDeltaParticipantsAddedToGroupThread(");
      bool __first = true;
      if (MessageMetadata != null && __isset.MessageMetadata) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("MessageMetadata: ");
        __sb.Append(MessageMetadata== null ? "<null>" : MessageMetadata.ToString());
      }
      if (AddedParticipants != null && __isset.AddedParticipants) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("AddedParticipants: ");
        __sb.Append(AddedParticipants);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
