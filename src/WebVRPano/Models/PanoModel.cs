﻿using System.Collections.Generic;
using System.Linq;

namespace WebVRPano.Models;

public class PanoModel
{
    private List<RootObject> Items { get; set; }

    public List<Pano> Panos { get; set; } = [];
    public string GlobalId { get; set; }
    public string SoortAanbod { get; set; }

    public PanoModel(List<RootObject> items, string globalId, string soortAanbod)
    {
        Items = items;
        GlobalId = globalId;
        SoortAanbod = soortAanbod;

        foreach (var item in items)
        {
            var pano = new Pano
            {
                Id = item.Id,
                Url = item.MediaItems.Where(mi => mi.Category == 23).Select(mi => mi.Url).FirstOrDefault() ?? string.Empty,
                Omschrijving = item.Omschrijving
            };

            if (Panos.Any(p => p.Omschrijving.Equals(item.Omschrijving)))
            {
                pano.Omschrijving += item.IndexNumber;
            }
            Panos.Add(pano);
        }
    }

    public PanoModel(List<RootObject> items)
    {
        Items = items;

        foreach (var item in items)
        {
            var pano = new Pano
            {
                Omschrijving = item.Omschrijving,
                Url = "http://www.funda.nl"
            };
            Panos.Add(pano);
        }
    }

    public int GetIndex(Pano pano)
    {
        for (int i = 0; i < Panos.Count; i++)
        {
            if (Panos[i] == pano) return i;
        }
        return -1;
    }

    public string GetWebsite()
        => SoortAanbod.ToLower() switch
        {
            "kantoor" or "bedrijfshal" or "winkel" or "horeca" or "bouwgrond" or "overig" => "fundainbusiness",
            _ => "funda",
        };
}

public class Pano
{
    public string Omschrijving { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string Id { get; set; } = string.Empty;
}
