using System;
using CxViewerAction.Entities;
using System.Collections.Generic;
using CxViewerAction.BaseInterfaces;
using CxViewerAction.Entities.WebServiceEntity;
using Common;

namespace CxViewerAction.Helpers
{
    public class GraphHelper
    {
        public static IGraph Convert(ReportQueryResult query)
        {
            IGraph outputGraph = new Graph();

            outputGraph.Severity = query.Severity;
            outputGraph.Paths = new List<GraphPath>();

            for (int i = 0; i < query.Paths.Count; i++)
            {
                ReportQueryItemResult path = query.Paths[i];
                outputGraph.AddNewPath(Convert(path, i));
            }

            return outputGraph;
        }

        public static IGraph Convert(TreeNodeData treeNode)
        {
            IGraph outputGraph = new Graph();

            CxViewerAction.CxVSWebService.CxWSResultPath[] paths = PerspectiveHelper.GetResultPathsForQuery(treeNode.ScanId, treeNode.Id);
            treeNode.QueryResult.Paths = ConvertAllNodesToPathes(paths, treeNode.Id, treeNode.QueryResult);
            outputGraph.Severity = treeNode.Severity;
            outputGraph.Paths = new List<GraphPath>();

            for (int i = 0; i < paths.Length; i++)
            {
                ReportQueryItemResult path = new ReportQueryItemResult()
                {
                    Column = paths[i].Nodes[0].Column,
                    FileName = paths[i].Nodes[0].FileName,
                    Line = paths[i].Nodes[0].Line,
                    NodeId = paths[i].Nodes[0].PathNodeId,
                    PathId = paths[i].PathId,
                    Query = treeNode.QueryResult
                };
                path.Paths = ConvertNodesToPathes(paths[i].Nodes, treeNode.QueryResult, path);
                outputGraph.AddNewPath(Convert(path, i));
            }

            return outputGraph;
        }

        public static List<ReportQueryItemPathResult> ConvertNodesToPathes(CxViewerAction.CxVSWebService.CxWSPathNode[] cxWSPathNode, ReportQueryResult queryResult, ReportQueryItemResult queryItemResult)
        {
            Logger.Create().Info("Converting nodes to pathes.");
            List<ReportQueryItemPathResult> list = new List<ReportQueryItemPathResult>();

            foreach (CxViewerAction.CxVSWebService.CxWSPathNode node in cxWSPathNode)
            {
                list.Add(new ReportQueryItemPathResult()
                {
                    Name = node.Name,
                    FileName = node.FileName,
                    Line = node.Line,
                    NodeId = node.PathNodeId,
                    Length = node.Length,
                    Column = node.Column,
                    Query = queryResult,
                    QueryItem = queryItemResult
                });
            }
            return list;
        }

        private static List<ReportQueryItemResult> ConvertAllNodesToPathes(CxViewerAction.CxVSWebService.CxWSResultPath[] cxWSPathNode, int resultId, ReportQueryResult queryResult)
        {
            Logger.Create().Info("Converting all nodes to pathes.");
            List<ReportQueryItemResult> list = new List<ReportQueryItemResult>();

            foreach (CxViewerAction.CxVSWebService.CxWSResultPath node in cxWSPathNode)
            {
                list.Add(new ReportQueryItemResult()
                {
                   Remark = node.Comment,
                   FileName = node.Nodes[0].FileName,
                   Line = node.Nodes[0].Line,
                   ResultId = resultId,
                   Query = queryResult,
                });
            }
            return list;
        }


        public static IGraphPath Convert(ReportQueryItemResult queryItem, int columnNumber)
        {
            IGraphPath outputPath = new GraphPath();
            outputPath.DirectFlow = new List<GraphItem>();

            for (int i = 0; i < queryItem.Paths.Count; i++)
            {
                if (i == 0 || i == queryItem.Paths.Count - 1)
                {
                    ReportQueryItemPathResult item = queryItem.Paths[i];
                    
                    GraphItem graphItem = (GraphItem)Convert(item, columnNumber, i == 0 ? 0 : 1);
                    graphItem.Parent = outputPath;

                    outputPath.DirectFlow.Add(graphItem);
                }
            }

            // To show two nodes in graph view if only one available
            if (outputPath.DirectFlow.Count == 1)
            {
                GraphItem graphItem = (GraphItem)Convert(queryItem.Paths[0], columnNumber, 1);
                graphItem.Parent = outputPath;

                outputPath.DirectFlow.Add(graphItem);
            }

            return outputPath;
        }

        private static IGraphItem Convert(ReportQueryItemPathResult item, int columnNumber, int rowNumber)
        {
            IGraphItem outputItem = new GraphItem();
            outputItem.Name = item.Name;
            outputItem.FileName = item.FileName;
            outputItem.Line = item.Line;
            outputItem.Column = item.Column;
            outputItem.Length = item.Length;
            outputItem.GraphX = columnNumber;
            outputItem.GraphY = rowNumber;

            outputItem.QueryItem = item.QueryItem;

            return outputItem;
        }
    }
}
