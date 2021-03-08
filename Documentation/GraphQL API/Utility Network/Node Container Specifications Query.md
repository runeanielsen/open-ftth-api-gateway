## Example Query

```graphql
query {
  utilityNetwork {
    nodeContainerSpecifications {
      id
      category
      name
      description
      deprecated
      manufacturerRefs
    }
  }
}
```

## Example result
```json
{
  "data": {
    "utilityNetwork": {
      "nodeContainerSpecifications": [
        {
          "id": "7fd8266e-44e1-46ee-a183-bc3068deadf3",
          "category": "ManHole",
          "name": "EK 378 400x800",
          "description": "37-EK 378 400x800mm",
          "deprecated": false,
          "manufacturerRefs": [
            "e845dc91-f3b9-407b-a622-2c300d43aaad"
          ]
        },
        {
          "id": "b93c3bcf-3013-4b6c-814d-06ff14d9139f",
          "category": "ManHole",
          "name": "EK 338 550x1165",
          "description": "37-EK 338 550x1165mm",
          "deprecated": false,
          "manufacturerRefs": [
            "e845dc91-f3b9-407b-a622-2c300d43aaad"
          ]
        },
        {
          "id": "8251e1d3-c586-4632-952a-41332aa61a47",
          "category": "ManHole",
          "name": "STAKKAbox 900x450",
          "description": "STAKKAbox MODULA 900x450mm",
          "deprecated": false,
          "manufacturerRefs": [
            "6b02e4aa-19f1-46a5-85e8-c1faab236ef0"
          ]
        },
        {
          "id": "0fb389b5-4bbd-4ebf-b506-bfc636001171",
          "category": "ManHole",
          "name": "STAKKAbox 600x450",
          "description": "STAKKAbox MODULA 600x450mm",
          "deprecated": false,
          "manufacturerRefs": [
            "6b02e4aa-19f1-46a5-85e8-c1faab236ef0"
          ]
        },
        {
          "id": "5eb03c1f-41f3-4cf2-81b0-13f91ad11432",
          "category": "CompressionConduitConnector",
          "name": "Straight 50mm",
          "description": "Straight 50mm (kompressionsmuffe)",
          "deprecated": false,
          "manufacturerRefs": [
            "e845dc91-f3b9-407b-a622-2c300d43aaad"
          ]
        },
        {
          "id": "a7bf7613-6ed3-4b38-a509-ea1c34e62660",
          "category": "ConduitClosure",
          "name": "Straight 50mm",
          "description": "50mm Straight In-line Elongated Enclosure",
          "deprecated": false,
          "manufacturerRefs": [
            "fd457db0-ad32-444c-9946-a9e5e8a14d17"
          ]
        },
        {
          "id": "7f2f1a7e-9e2d-45c4-958a-ce049a69a9a3",
          "category": "CompressionConduitConnector",
          "name": "Straight 40mm",
          "description": "Straight 40mm (kompressionsmuffe)",
          "deprecated": false,
          "manufacturerRefs": [
            "e845dc91-f3b9-407b-a622-2c300d43aaad"
          ]
        },
        {
          "id": "6c1c9ab8-b1f2-4021-bece-d9b4f65c6723",
          "category": "ManHole",
          "name": "EK 328 800x800",
          "description": "37-EK 328 800x800mm",
          "deprecated": false,
          "manufacturerRefs": [
            "e845dc91-f3b9-407b-a622-2c300d43aaad"
          ]
        },
        {
          "id": "ded31f47-9161-4080-ae82-1251ae2fc8c0",
          "category": "ConduitClosure",
          "name": "Branch box 50mm",
          "description": "Branch box 50mm",
          "deprecated": false,
          "manufacturerRefs": [
            "fd457db0-ad32-444c-9946-a9e5e8a14d17"
          ]
        }
      ]
    }
  }
}
```
