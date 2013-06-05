﻿using System.Linq;
using NMaier.SimpleDlna.Server.Metadata;

namespace NMaier.SimpleDlna.Server.Views
{
  internal class LargeView : BaseView
  {
    private const long MIN_SIZE = 300 * 1024 * 1024;


    public override string Description
    {
      get {
        return "Show only large files";
      }
    }
    public override string Name
    {
      get {
        return "large";
      }
    }


    private static void ProcessFolder(IMediaFolder root)
    {
      foreach (var f in root.ChildFolders) {
        ProcessFolder(f);
      }
      foreach (var f in root.ChildItems.ToList()) {
        var i = f as IMetaInfo;
        if (i == null) {
          continue;
        }
        if (i.InfoSize.HasValue && i.InfoSize.Value >= MIN_SIZE) {
          continue;
        }
        root.RemoveResource(f);
      }
    }


    public override IMediaFolder Transform(IMediaFolder root)
    {
      root = new VirtualClonedFolder(root);
      ProcessFolder(root);
      return root;
    }
  }
}
