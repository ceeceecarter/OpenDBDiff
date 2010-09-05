using System;
using System.Collections.Generic;
using System.Text;
using DBDiff.Schema.SQLServer.Model;

namespace DBDiff.Schema.SQLServer.Compare
{
    internal class CompareFileGroups:CompareBase<FileGroup>
    {
        public static FileGroups GenerateDiferences(FileGroups CamposOrigen, FileGroups CamposDestino)
        {
            foreach (FileGroup node in CamposDestino)
            {
                if (!CamposOrigen.Exists(node.FullName))
                {
                    FileGroup newNode = node.Clone(CamposOrigen.Parent);
                    newNode.Status = StatusEnum.ObjectStatusType.CreateStatus;
                    CamposOrigen.Add(newNode);
                }
                else
                {
                    if (!FileGroup.Compare(node, CamposOrigen[node.FullName]))
                    {
                        FileGroup newNode = node.Clone(CamposOrigen.Parent);
                        newNode.Status = StatusEnum.ObjectStatusType.AlterStatus;
                        CamposOrigen[node.FullName] = newNode;
                    }
                }
            }
            
            MarkDrop(CamposOrigen, CamposDestino);

            return CamposOrigen;
        }
    }
}