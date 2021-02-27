## Example Query

```graphql
query {
  schematic {
    buildDiagram(
      routeNetworkElementId: "020c3eb8-13cc-4c0e-a331-c7610f996a52"
    ) {
      envelope {
        minX
        maxX
        minY
        maxY
      }
      diagramObjects {
        refId
        refClass
        geometry {
          type
          coordinates
        }
        style
        label
      }
    }
  }
}
```

## Example result
```json
{
  "data": {
    "schematic": {
      "buildDiagram": {
        "envelope": {
          "minX": -0.01,
          "maxX": 0.04,
          "minY": 0,
          "maxY": 0.0071
        },
        "diagramObjects": [
          {
            "refId": "a2254f4a-d740-4436-b49f-62d86d3097f8",
            "refClass": "SpanStructure",
            "geometry": {
              "type": "Polygon",
              "coordinates": "[[[0.0,0.0],[0.03,0.0],[0.03,0.0066],[0.0,0.0066],[0.0,0.0]]]"
            },
            "style": "OuterConduitOrange",
            "label": null
          },
          {
            "refId": null,
            "refClass": null,
            "geometry": {
              "type": "Point",
              "coordinates": "[0.0,0.0013]"
            },
            "style": "VestTerminalLabel",
            "label": "HH-1"
          },
          {
            "refId": null,
            "refClass": null,
            "geometry": {
              "type": "Point",
              "coordinates": "[0.0,0.0023]"
            },
            "style": "VestTerminalLabel",
            "label": "HH-1"
          },
          {
            "refId": null,
            "refClass": null,
            "geometry": {
              "type": "Point",
              "coordinates": "[0.0,0.0033]"
            },
            "style": "VestTerminalLabel",
            "label": "HH-1"
          },
          {
            "refId": null,
            "refClass": null,
            "geometry": {
              "type": "Point",
              "coordinates": "[0.0,0.0043]"
            },
            "style": "VestTerminalLabel",
            "label": "HH-1"
          },
          {
            "refId": null,
            "refClass": null,
            "geometry": {
              "type": "Point",
              "coordinates": "[0.0,0.0053]"
            },
            "style": "VestTerminalLabel",
            "label": "HH-1"
          },
          {
            "refId": null,
            "refClass": null,
            "geometry": {
              "type": "Point",
              "coordinates": "[0.03,0.0013]"
            },
            "style": "EastTerminalLabel",
            "label": "HH-10"
          },
          {
            "refId": null,
            "refClass": null,
            "geometry": {
              "type": "Point",
              "coordinates": "[0.03,0.0023]"
            },
            "style": "EastTerminalLabel",
            "label": "HH-10"
          },
          {
            "refId": null,
            "refClass": null,
            "geometry": {
              "type": "Point",
              "coordinates": "[0.03,0.0033]"
            },
            "style": "EastTerminalLabel",
            "label": "HH-10"
          },
          {
            "refId": null,
            "refClass": null,
            "geometry": {
              "type": "Point",
              "coordinates": "[0.03,0.0043]"
            },
            "style": "EastTerminalLabel",
            "label": "HH-10"
          },
          {
            "refId": null,
            "refClass": null,
            "geometry": {
              "type": "Point",
              "coordinates": "[0.03,0.0053]"
            },
            "style": "EastTerminalLabel",
            "label": "HH-10"
          },
          {
            "refId": "dd18cbd5-141c-46d1-9760-11eaf84301c8",
            "refClass": "SpanStructure",
            "geometry": {
              "type": "Polygon",
              "coordinates": "[[[0.0,0.0009],[0.03,0.0009],[0.03,0.0017],[0.0,0.0017],[0.0,0.0009]]]"
            },
            "style": "InnerConduitBlue",
            "label": null
          },
          {
            "refId": "f2da296d-e4db-42ca-9103-0204eb34a7dd",
            "refClass": "SpanStructure",
            "geometry": {
              "type": "Polygon",
              "coordinates": "[[[0.0,0.0019],[0.03,0.0019],[0.03,0.0027],[0.0,0.0027],[0.0,0.0019]]]"
            },
            "style": "InnerConduitYellow",
            "label": null
          },
          {
            "refId": "64713b5f-9079-42fd-b7e8-42375861c617",
            "refClass": "SpanStructure",
            "geometry": {
              "type": "Polygon",
              "coordinates": "[[[0.0,0.0029],[0.03,0.0029],[0.03,0.0037],[0.0,0.0037],[0.0,0.0029]]]"
            },
            "style": "InnerConduitWhite",
            "label": null
          },
          {
            "refId": "c6f430c9-721b-41c8-999e-e91e8ca02afd",
            "refClass": "SpanStructure",
            "geometry": {
              "type": "Polygon",
              "coordinates": "[[[0.0,0.0039],[0.03,0.0039],[0.03,0.0047],[0.0,0.0047],[0.0,0.0039]]]"
            },
            "style": "InnerConduitGreen",
            "label": null
          },
          {
            "refId": "cec760c1-3eff-4c76-9ff5-8dc8350423e1",
            "refClass": "SpanStructure",
            "geometry": {
              "type": "Polygon",
              "coordinates": "[[[0.0,0.0049],[0.03,0.0049],[0.03,0.0057],[0.0,0.0057],[0.0,0.0049]]]"
            },
            "style": "InnerConduitBlack",
            "label": null
          },
          {
            "refId": null,
            "refClass": null,
            "geometry": {
              "type": "Point",
              "coordinates": "[0.0,0.0071]"
            },
            "style": "SpanEquipmentLabel",
            "label": "Ø40 5x10"
          }
        ]
      }
    }
  }
}  
```
