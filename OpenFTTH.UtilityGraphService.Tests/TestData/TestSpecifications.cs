﻿using OpenFTTH.Results;
using OpenFTTH.CQRS;
using OpenFTTH.Util;
using OpenFTTH.UtilityGraphService.API.Commands;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.API.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OpenFTTH.TestData
{
    public class TestSpecifications
    {
        private static readonly object _myLock = new object();

        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;

        public TestSpecifications(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        public static Guid Manu_GMPlast = Guid.Parse("47e87d16-a1f0-488a-8c3e-cb3a4f3e8926");
        public static Guid Manu_Emtelle = Guid.Parse("fd457db0-ad32-444c-9946-a9e5e8a14d17");
        public static Guid Manu_Fiberpowertech = Guid.Parse("e845dc91-f3b9-407b-a622-2c300d43aaad");
        public static Guid Manu_Cubis = Guid.Parse("6b02e4aa-19f1-46a5-85e8-c1faab236ef0");
        public static Guid Manu_CommScope = Guid.Parse("6f729864-ea2a-4ddf-b370-3271ef81879c");
        public static Guid Manu_HuberSuhner = Guid.Parse("52d8191b-8cad-4584-a133-99dc252e5193");
        public static Guid Manu_Nokia = Guid.Parse("8f45274a-7740-43fd-98c9-ca44d00d6ff9");

        public static Guid Well_Cubis_STAKKAbox_MODULA_600x450 = Guid.Parse("0fb389b5-4bbd-4ebf-b506-bfc636001171");
        public static Guid Well_Cubis_STAKKAbox_MODULA_900x450 = Guid.Parse("8251e1d3-c586-4632-952a-41332aa61a47");

        public static Guid Well_Fiberpowertech_37_EK_378_400x800 = Guid.Parse("7fd8266e-44e1-46ee-a183-bc3068deadf3");
        public static Guid Well_Fiberpowertech_37_EK_338_550x1165 = Guid.Parse("b93c3bcf-3013-4b6c-814d-06ff14d9139f");
        public static Guid Well_Fiberpowertech_37_EK_328_800x800 = Guid.Parse("6c1c9ab8-b1f2-4021-bece-d9b4f65c6723");

        // 50mm Straight In-line Elongated Enclosure (gammel model)
        public static Guid Conduit_Closure_Emtelle_Straight_In_line = Guid.Parse("a7bf7613-6ed3-4b38-a509-ea1c34e62660");

        // Branch box 50mm (ny model)
        public static Guid Conduit_Closure_Emtelle_Branch_Box = Guid.Parse("ded31f47-9161-4080-ae82-1251ae2fc8c0");

        // Kompesssionsmuffer
        public static Guid Conduit_Connector_Fiberpowertech_Straight_40 = Guid.Parse("7f2f1a7e-9e2d-45c4-958a-ce049a69a9a3");
        public static Guid Conduit_Connector_Fiberpowertech_Straight_50 = Guid.Parse("5eb03c1f-41f3-4cf2-81b0-13f91ad11432");

        // Span Structures
        public static Guid Ø7_Blue = Guid.Parse("94d5bc20-31da-4191-b4dc-9a701bba83d6");
        public static Guid Ø7_Yellow = Guid.Parse("6416641c-1c31-4e6f-9bc4-41a435915a69");
        public static Guid Ø7_White = Guid.Parse("6c861c63-0134-45ac-9e1e-c982b543e74e");
        public static Guid Ø7_Green = Guid.Parse("55af46cb-5c53-4046-a4d6-0069ad60bbf8");
        public static Guid Ø7_Black = Guid.Parse("b96a1ecb-4665-4929-a016-82ee48cae57e");
        public static Guid Ø7_Red = Guid.Parse("ef1c6787-eb76-4941-96e0-4ca2fac5b57b");
        public static Guid Ø7_Orange = Guid.Parse("def493b9-71a1-43c5-9f09-02e8229bc785");
        public static Guid Ø7_Pink = Guid.Parse("1bb0ee9b-3f41-44f0-8f5b-ccae8cb63a23");
        public static Guid Ø7_Silver = Guid.Parse("16c77f06-3439-4843-8ff9-76bb3a7c8b20");
        public static Guid Ø7_Brown = Guid.Parse("4e9373e8-c6bd-4a85-856d-6f2ef57f6833");
        public static Guid Ø7_Turquoise = Guid.Parse("72d88e01-178a-4656-9fe5-8819544c94c9");
        public static Guid Ø7_Violet = Guid.Parse("3672dd91-886a-444e-93a2-afc0d44d6541");

        public static Guid Ø10_Blue = Guid.Parse("980a2a21-cf40-4b70-91ae-69af79be9e80");
        public static Guid Ø10_Yellow = Guid.Parse("779a8d88-1c52-4fca-b2d5-0aabfa652393");
        public static Guid Ø10_White = Guid.Parse("ec75555f-33ea-432f-9235-d1f111cebd68");
        public static Guid Ø10_Green = Guid.Parse("c1734de6-1ca2-4b2f-af74-86c6ca45f6e5");
        public static Guid Ø10_Black = Guid.Parse("30f3f962-274c-43fc-951b-f3e2213f9ba7");
        public static Guid Ø10_Red = Guid.Parse("2ef21422-fc35-4c87-8051-b235568c1def");
        public static Guid Ø10_Orange = Guid.Parse("0aa07b3a-8168-4ab8-8ccf-62c163e5be28");
        public static Guid Ø10_Pink = Guid.Parse("f9d5f15e-d4f2-4d53-9fce-7808a579e853");
        public static Guid Ø10_Silver = Guid.Parse("7e21f619-039b-4a58-b43c-ee5496f0cbb2");
        public static Guid Ø10_Brown = Guid.Parse("b4cde3bf-56ff-43f3-a62a-270cf9afa24d");
        public static Guid Ø10_Turquoise = Guid.Parse("9d05b556-2eb4-4a30-89d6-4b813c10dabe");
        public static Guid Ø10_Violet = Guid.Parse("c09ca8f5-cd37-4cd8-9d32-b6274f3c2c64");

        public static Guid Ø12_Orange = Guid.Parse("5e0bef90-6838-49a7-9063-7d099a06e23d");
        public static Guid Ø12_Red = Guid.Parse("f90dfced-3244-4cde-9839-2ca8b49a4483");

        public static Guid Ø32_Orange = Guid.Parse("e7dea74e-df7f-4a0d-a752-98f9e02be1a8");
        public static Guid Ø40_Orange = Guid.Parse("ac417fea-b6f6-4a5a-9c9e-10ee05ecbf56");
        public static Guid Ø40_Red = Guid.Parse("be4deb0f-8d15-49ba-bbeb-fafb4ed66de5");
        public static Guid Ø50_Orange = Guid.Parse("7960355a-4dab-4d60-b3a5-e20ac4301176");
        public static Guid Ø110_Red = Guid.Parse("e078e830-f79d-4220-bd9a-87ed7cf81f1d");

        // Multi Conduit Span Equipment
        public static Guid Multi_Ø32_3x10 = Guid.Parse("b11a4fce-2116-4437-9108-3ca467124d99");

        public static Guid Multi_Ø40_5x10 = Guid.Parse("7ca9dcbb-524f-4d61-945c-16bf2679326e");
        public static Guid Multi_Ø40_5x10_Red = Guid.Parse("9262c4f2-06fc-4315-be71-12fd9c57aedd");
        public static Guid Multi_Ø40_5x10_BlackBlack = Guid.Parse("21294bdc-9daa-4ac1-b118-1dca603f1c3f");

        public static Guid Multi_Ø40_6x10 = Guid.Parse("f8d15ef6-b07f-440b-8357-4c7a3f84f156");
        public static Guid Multi_Ø40_12x7 = Guid.Parse("eac5660c-446b-4404-b0af-01032ebf94da");

        public static Guid Multi_Ø50_10x10 = Guid.Parse("1c2a1e9e-03e6-4eb9-ae89-e723fea1e59c");
        public static Guid Multi_Ø50_10x7_5x10 = Guid.Parse("5c3e36f7-1278-4613-9f67-fbcf93195fa7");
        public static Guid Multi_Ø50_12x7_5x10 = Guid.Parse("36f0deaf-0d77-4cae-be06-1e6e0cf84ae2");
        public static Guid Multi_Ø50_12x7_5x10_BlueYellow = Guid.Parse("dc83bdd3-142b-49ff-8a80-d8d7e1d794b3");
        public static Guid Multi_Ø50_12x7_5x10_GreenWhite = Guid.Parse("2fe1a566-6477-4f24-b7df-e242bd6c7d7d");

        public static Guid Flex_Ø40_Red = Guid.Parse("6df48525-ac10-4b02-b1cd-05283b549ab2");
        public static Guid Tomrør_Ø110_Red = Guid.Parse("233e77b1-b580-4093-91c1-48ac636cb300");
        public static Guid Tomrør_Ø40_Orange = Guid.Parse("9c933058-1ea6-475f-930a-b389c421023d");
        public static Guid Tomrør_Ø50_Orange = Guid.Parse("5c805317-8216-4b00-88c3-6ec9d542f831");

        // Single Conduit SpanEquipment
        public static Guid SingleConduit_Ø7_Blue = Guid.Parse("abaaf52d-2b74-4f45-b7a7-700969a9b13e");
        public static Guid SingleConduit_Ø7_Yellow = Guid.Parse("49182083-3f67-4991-aa0b-4a83f8c9faca");
        public static Guid SingleConduit_Ø7_White = Guid.Parse("c6eee17a-85e5-4665-9ca4-83bb57fef025");
        public static Guid SingleConduit_Ø7_Green = Guid.Parse("2ebad22f-ff39-42bb-8dad-b7cef8122caf");
        public static Guid SingleConduit_Ø7_Black = Guid.Parse("8597992f-8d7c-46cf-b962-901417899c05");
        public static Guid SingleConduit_Ø7_Red = Guid.Parse("6d4c9ce6-dc1b-4b63-92cb-c9b734acc2af");
        public static Guid SingleConduit_Ø7_Orange = Guid.Parse("0dd594fa-7b5a-4700-9032-580eaef9bc04");
        public static Guid SingleConduit_Ø7_Pink = Guid.Parse("144d1546-e52c-41c3-b51b-2ace90ba3494");
        public static Guid SingleConduit_Ø7_Silver = Guid.Parse("ac6b6f0a-6d38-4787-983f-9889490ad15f");
        public static Guid SingleConduit_Ø7_Brown = Guid.Parse("ef4c187d-db67-4f36-9767-8394a11c14e4");
        public static Guid SingleConduit_Ø7_Turquoise = Guid.Parse("e96f2c04-5a15-476b-aec4-a99527984c8b");
        public static Guid SingleConduit_Ø7_Violet = Guid.Parse("0c69a8cf-3eb1-41c1-876c-bd658b077eda");

        public static Guid SingleConduit_Ø10_Blue = Guid.Parse("22869ff0-e5e5-4269-b92a-b38502c80b04");
        public static Guid SingleConduit_Ø10_Yellow = Guid.Parse("aac8a871-f32c-420b-ab78-637762451c33");
        public static Guid SingleConduit_Ø10_White = Guid.Parse("ef14f165-9fd7-4413-a49b-f573e768427e");
        public static Guid SingleConduit_Ø10_Green = Guid.Parse("5f5b702a-06fd-4bfe-ab13-ce829f29477b");
        public static Guid SingleConduit_Ø10_Black = Guid.Parse("44875813-b7f7-4763-88f5-bb8c9f53ec54");
        public static Guid SingleConduit_Ø10_Red = Guid.Parse("6e254bfe-9e98-4f3c-9a23-c34a89066611");
        public static Guid SingleConduit_Ø10_Orange = Guid.Parse("16d65e91-427d-4875-8ebe-1aa407e290c2");
        public static Guid SingleConduit_Ø10_Pink = Guid.Parse("e81c89f5-9cf0-4970-9f81-37dbc3333086");
        public static Guid SingleConduit_Ø10_Silver = Guid.Parse("5a72e1ce-5ecf-42cd-ac56-ac1eec42bb24");
        public static Guid SingleConduit_Ø10_Brown = Guid.Parse("fae6deb1-cbc4-4848-9f98-cdddb5442a17");
        public static Guid SingleConduit_Ø10_Turquoise = Guid.Parse("65cc9205-3884-4f52-abac-2fcbadfc1674");
        public static Guid SingleConduit_Ø10_Violet = Guid.Parse("fbfa0de1-1e1d-4592-bb90-0edad2f9ee46");

        public static Guid SingleConduit_Ø12_Red = Guid.Parse("0b6be410-a6e3-4696-9964-3aff3e827dc8");
        public static Guid SingleConduit_Ø12_Orange = Guid.Parse("12c5e369-9fcc-49c7-81f5-208769501b7d");

        // Customer span equipments
        public static Guid CustomerConduit_Ø12_Orange = Guid.Parse("ddd86873-9d6c-4741-a406-084c628314db");


        // Fiber cables
        public static Guid FiberCable_Jacket = Guid.Parse("f95154f2-7df6-4149-bbb7-587c40fed385");
        public static Guid FiberCable_Fiber = Guid.Parse("162faf17-dad7-4838-81a9-73b5ab8f61c2");

        public static Guid FiberCable_2Fiber = Guid.Parse("8164e388-ca59-4ad1-8317-44b601c39e2a");
        public static Guid FiberCable_4Fiber = Guid.Parse("39756e03-bc7e-4341-9a73-672f1c6849b4");
        public static Guid FiberCable_12Fiber = Guid.Parse("2ed3de4f-178e-4f61-bf30-348f2c9781b2");
        public static Guid FiberCable_24Fiber = Guid.Parse("cfd395b6-288c-4fe8-90d7-a523d7a862db");
        public static Guid FiberCable_48Fiber = Guid.Parse("64b3ba07-dd2f-4880-a0e0-24b1d0faa771");
        public static Guid FiberCable_72Fiber = Guid.Parse("703afeeb-a32e-4600-a9e1-0ed50fabfdbe");
        public static Guid FiberCable_96Fiber = Guid.Parse("9ae8da66-4643-4fab-bce9-5a01545640a6");
        public static Guid FiberCable_144Fiber = Guid.Parse("d50fb652-5092-44e3-b5f5-1ff585b0c18d");
        public static Guid FiberCable_192Fiber = Guid.Parse("6df0a7c2-e10e-42eb-b0fb-8371caaa043e");
        public static Guid FiberCable_216Fiber = Guid.Parse("e1cb1f5a-201a-4cf7-8420-a6f5bbafd7aa");
        public static Guid FiberCable_288Fiber = Guid.Parse("0450545d-1698-4d68-a85f-532270a7059a");


        // Racks
        public static Guid Rack_ESTI = Guid.Parse("b72523d7-4a55-489e-8901-a9fdf9a7d471");
        public static Guid Rack_Super = Guid.Parse("d79103eb-4714-4ede-975e-ebefd0297d69");


        // Terminal Equipment Structure
        public static Guid SpliceTray_Uknown4Pin = Guid.Parse("19b70c93-fcfa-4266-b5f8-d736e7a1c36f");
        public static Guid SpliceTray_Uknown12Pin = Guid.Parse("42e126cf-9654-4f0e-b3c2-15bce380fd4e");
        public static Guid SpliceTray_Uknown24Pin = Guid.Parse("629e7a6c-7326-4cfb-bf8b-df1f78b7473e");
        public static Guid SpliceTray_SE12Pin = Guid.Parse("fdd67d8c-de49-46d6-ac88-af392f539019");
        public static Guid SpliceTray_SC12Pin = Guid.Parse("4a15dcc1-4477-4c9d-bf5e-e530480ce822");

        public static Guid SplicePatchTray_LX12UPC12APC = Guid.Parse("adabaab0-24bd-4ecf-b703-932c407cfba8");
        public static Guid SplicePatchTray_GPS2_12SC = Guid.Parse("8e36b878-5f14-40f3-b088-f04f2ffb03d7");
        public static Guid SplicePatchTray_GPS2_24LC = Guid.Parse("e24b4f50-24f6-4a33-8b56-cf9a52dce522");

        public static Guid GSS_4x1_2_Splitter = Guid.Parse("52e3f107-872b-428b-8eff-af29a08f2e3a");

        public static Guid LISA_1_32_Splitter = Guid.Parse("7f5b81b8-f0db-45ca-8cbf-6f7e458636b0");

        public static Guid OLT_LineCard8Port = Guid.Parse("79e5653e-a06a-4921-8b43-2dbec1e0e914");
        public static Guid OLT_LineCard16Port = Guid.Parse("2238245d-8a85-4a49-8499-cd9c4045cd00");
        public static Guid OLT_InterfaceModule = Guid.Parse("5530de70-2a4c-4131-8e40-5617bcbfd3f5");


        public static Guid LGX_WDMType1 = Guid.Parse("5602395b-f3f0-4e99-adb1-77901f4c711b");
        public static Guid LGX_WDMType2 = Guid.Parse("fe781a46-4402-4d7b-9518-48dcd36e9128");



        // Terminal Equipments
        public static Guid SpliceClosure_VMC_12Tray = Guid.Parse("c27377df-f5d0-483c-bc35-2ce8ab56c31b");
        public static Guid SpliceClosure_VMC_24Tray = Guid.Parse("420cdc14-5eaf-4d28-ade2-1cb5a940d818");
        public static Guid SpliceClosure_VMC_LZ = Guid.Parse("6ed9bc08-51b7-4c0c-a286-16f27f2f3ffb");
        public static Guid SpliceClosure_3M_72Fiber = Guid.Parse("c20fb96a-18c7-4730-aae2-e2e5882006d9");
        public static Guid SpliceClosure_Uknown12Fiber = Guid.Parse("a3de806d-8e3b-4280-8e12-ff875bf87469");
        public static Guid SpliceClosure_Uknown72Fiber = Guid.Parse("411b565b-2704-4416-bfaa-09e1faa62f8b");
        public static Guid SpliceClosure_BUDI1S_16SCTrays = Guid.Parse("b982398f-d546-41ab-a5d1-10048d5b9db6");
        public static Guid SpliceClosure_BUDI1S_6SETrays = Guid.Parse("a27fd2e9-9c5e-459d-b3e2-10dd8932cca0");
        public static Guid SpliceClosure_BUDI2S_1SETrays = Guid.Parse("57a1d8ae-38c5-499e-b55a-5afc00687a20");
        public static Guid SpliceClosure_FTUO = Guid.Parse("a469960e-f650-487e-b16b-ce094ef4d9e6");
        public static Guid SpliceClosure_FIST = Guid.Parse("7a038a46-297d-490c-8796-42b44d1218e0");
        public static Guid SpliceClosure_FOSC400 = Guid.Parse("5d95fe82-c563-47ff-8356-81d63bb512ee");

        public static Guid CustomerTermination = Guid.Parse("b0a3e179-ef1a-405c-8b4e-0082d8fc8c3d");

        public static Guid Subrack_LISA_APC_UPC = Guid.Parse("778b9d6f-7add-40eb-ae9d-da9660dc1799");
        public static Guid Subrack_GPS_72_SC = Guid.Parse("aa8027fc-25d6-498e-98e6-4eb7d634070c");
        public static Guid Subrack_GPS_144_LC = Guid.Parse("d1748de5-de10-45d9-a2a9-1b5b344bf159");


        public static Guid GSS_24_Splitters = Guid.Parse("87e7932e-5192-4914-b3eb-f3564348f682");

        public static Guid LISA_SplitterHolder = Guid.Parse("8a840669-2b30-4f3c-a781-4cc4667b4527");

        public static Guid OLT = Guid.Parse("00bb0c1d-f540-4000-af1d-0d180ce0d3bb");

        public static Guid LGX_Holder = Guid.Parse("6f648aae-0fb8-4a41-9de4-617573da26a9");



        public OpenFTTH.Results.Result<TestSpecifications> Run()
        {
            lock (_myLock)
            {
                var manufacturerQueryResult = _queryDispatcher.HandleAsync<GetManufacturer, Result<LookupCollection<Manufacturer>>>(new GetManufacturer()).Result;

                if (manufacturerQueryResult.Value.ContainsKey(Manu_GMPlast))
                    return OpenFTTH.Results.Result.Fail("Test specification already present in system");

                AddManufactures();

                AddNodeContainerSpecifications();

                AddSpanStructureSpecifications();

                AddSpanEquipmentSpecifications();

                AddFiberCableSpecifications();

                AddRackSpecifications();

                AddTerminalStructureSpecifications();

                AddTerminalEquipmentSpecifications();

                Thread.Sleep(100);

                return OpenFTTH.Results.Result.Ok(this);
            }
        }

        private void AddNodeContainerSpecifications()
        {
            // Man Holes
            AddSpecification(new NodeContainerSpecification(Well_Cubis_STAKKAbox_MODULA_600x450, "ManHole", "STAKKAbox 600x450")
            {
                Description = "STAKKAbox MODULA 600x450mm",
                ManufacturerRefs = new Guid[] { Manu_Cubis }
            });

            AddSpecification(new NodeContainerSpecification(Well_Cubis_STAKKAbox_MODULA_900x450, "ManHole", "STAKKAbox 900x450")
            {
                Description = "STAKKAbox MODULA 900x450mm",
                ManufacturerRefs = new Guid[] { Manu_Cubis }
            });

            AddSpecification(new NodeContainerSpecification(Well_Fiberpowertech_37_EK_378_400x800, "ManHole", "EK 378 400x800")
            {
                Description = "37-EK 378 400x800mm",
                ManufacturerRefs = new Guid[] { Manu_Fiberpowertech }
            });

            AddSpecification(new NodeContainerSpecification(Well_Fiberpowertech_37_EK_338_550x1165, "ManHole", "EK 338 550x1165")
            {
                Description = "37-EK 338 550x1165mm",
                ManufacturerRefs = new Guid[] { Manu_Fiberpowertech }
            });

            AddSpecification(new NodeContainerSpecification(Well_Fiberpowertech_37_EK_328_800x800, "ManHole", "EK 328 800x800")
            {
                Description = "37-EK 328 800x800mm",
                ManufacturerRefs = new Guid[] { Manu_Fiberpowertech }
            });

            // Conduit Closures
            AddSpecification(new NodeContainerSpecification(Conduit_Closure_Emtelle_Branch_Box, "ConduitClosure", "Branch box 50mm")
            {
                Description = "Branch box 50mm",
                ManufacturerRefs = new Guid[] { Manu_Emtelle }
            });

            AddSpecification(new NodeContainerSpecification(Conduit_Closure_Emtelle_Straight_In_line, "ConduitClosure", "Straight 50mm")
            {
                Description = "50mm Straight In-line Elongated Enclosure",
                ManufacturerRefs = new Guid[] { Manu_Emtelle }
            });

            // Conduit Compression Connectors
            AddSpecification(new NodeContainerSpecification(Conduit_Connector_Fiberpowertech_Straight_40, "CompressionConduitConnector", "Straight 40mm")
            {
                Description = "Straight 40mm (kompressionsmuffe)",
                ManufacturerRefs = new Guid[] { Manu_Fiberpowertech }
            });

            AddSpecification(new NodeContainerSpecification(Conduit_Connector_Fiberpowertech_Straight_50, "CompressionConduitConnector", "Straight 50mm")
            {
                Description = "Straight 50mm (kompressionsmuffe)",
                ManufacturerRefs = new Guid[] { Manu_Fiberpowertech }
            });

        }

        private void AddSpanEquipmentSpecifications()
        {
            AddSpecification(new SpanEquipmentSpecification(Multi_Ø32_3x10, "Conduit", "Ø32 3x10",
                new SpanStructureTemplate(Ø32_Orange, 1, 1,
                    new SpanStructureTemplate[] {
                        new SpanStructureTemplate(Ø10_Blue, 2, 1, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Yellow, 2, 2, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_White, 2, 3, Array.Empty<SpanStructureTemplate>())
                    })
                )
            {
                Description = "ø32 mm Multirør 3x10",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true
            });


            // Ø40

            AddSpecification(new SpanEquipmentSpecification(Multi_Ø40_5x10, "Conduit", "Ø40 5x10",
                new SpanStructureTemplate(Ø40_Orange, 1, 1,
                    new SpanStructureTemplate[] {
                        new SpanStructureTemplate(Ø10_Blue, 2, 1, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Yellow, 2, 2, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_White, 2, 3, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Green, 2, 4, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Black, 2, 5, Array.Empty<SpanStructureTemplate>())
                    })
                )
            {
                Description = "ø40 mm Multirør 5x10",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true
            });

            AddSpecification(new SpanEquipmentSpecification(Multi_Ø40_5x10_Red, "Conduit", "Ø40 5x10",
               new SpanStructureTemplate(Ø40_Orange, 1, 1,
                   new SpanStructureTemplate[] {
                        new SpanStructureTemplate(Ø10_Blue, 2, 1, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Yellow, 2, 2, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_White, 2, 3, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Green, 2, 4, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Red, 2, 5, Array.Empty<SpanStructureTemplate>())
                   })
               )
            {
                Description = "ø40 mm Multirør 5x10 (Blå, Gul, Hvid, Grøn, Rød)",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true
            });

            AddSpecification(new SpanEquipmentSpecification(Multi_Ø40_5x10_BlackBlack, "Conduit", "Ø40 5x10",
               new SpanStructureTemplate(Ø40_Orange, 1, 1,
                   new SpanStructureTemplate[] {
                        new SpanStructureTemplate(Ø10_Blue, 2, 1, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_White, 2, 2, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Yellow, 2, 3, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Black, 2, 4, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Black, 2, 5, Array.Empty<SpanStructureTemplate>())
                   })
               )
            {
                Description = "ø40 mm Multirør 5x10 (Blå, Hvid, Grøn, Sort, Sort)",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true
            });



            AddSpecification(new SpanEquipmentSpecification(Multi_Ø40_6x10, "Conduit", "Ø40 6x10",
                new SpanStructureTemplate(Ø40_Orange, 1, 1,
                    new SpanStructureTemplate[] {
                        new SpanStructureTemplate(Ø10_Blue, 2, 1, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Yellow, 2, 2, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_White, 2, 3, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Green, 2, 4, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Black, 2, 5, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Red, 2, 6, Array.Empty<SpanStructureTemplate>())
                    })
                )
            {
                Description = "ø40 mm Multirør 6x10",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true
            });

            // Ø 40
            AddSpecification(new SpanEquipmentSpecification(Multi_Ø50_10x10, "Conduit", "Ø50 10x10",
                new SpanStructureTemplate(Ø50_Orange, 1, 1,
                    new SpanStructureTemplate[] {
                        new SpanStructureTemplate(Ø10_Blue, 2, 1, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Yellow, 2, 2, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_White, 2, 3, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Green, 2, 4, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Black, 2, 5, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Red, 2, 6, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Orange, 2, 7, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Pink, 2, 8, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Silver, 2, 9, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Brown, 2, 10, Array.Empty<SpanStructureTemplate>())
                    })
                )
            {
                Description = "ø50 mm Multirør 10x10",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true
            });

            AddSpecification(new SpanEquipmentSpecification(Multi_Ø40_12x7, "Conduit", "Ø40 12x7",
                new SpanStructureTemplate(Ø40_Orange, 1, 1,
                    new SpanStructureTemplate[] {
                        new SpanStructureTemplate(Ø7_Blue, 2, 1, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Yellow, 2, 2, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_White, 2, 3, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Green, 2, 4, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Black, 2, 5, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Red, 2, 6, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Orange, 2, 7, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Pink, 2, 8, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Silver, 2, 9, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Brown, 2, 10, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Turquoise, 2, 11, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Violet, 2, 12, Array.Empty<SpanStructureTemplate>())
                    })
                )
            {
                Description = "ø40 mm Multirør 12x7 standard farver",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true
            });

            AddSpecification(new SpanEquipmentSpecification(Multi_Ø50_10x7_5x10, "Conduit", "Ø50 5x10+10x7",
                new SpanStructureTemplate(Ø50_Orange, 1, 1,
                    new SpanStructureTemplate[] {
                        new SpanStructureTemplate(Ø10_Blue, 2, 1, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Yellow, 2, 2, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_White, 2, 3, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Green, 2, 4, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Black, 2, 5, Array.Empty<SpanStructureTemplate>()),

                        new SpanStructureTemplate(Ø7_Blue, 2, 6, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Yellow, 2, 7, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_White, 2, 8, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Green, 2, 9, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Black, 2, 10, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Red, 2, 11, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Orange, 2, 12, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Pink, 2, 13, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Silver, 2, 14, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Brown, 2, 15, Array.Empty<SpanStructureTemplate>()),
                    })
                )
            {
                Description = "ø50 mm Multirør 5x10 + 10x7 standard farver",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true
            });

            AddSpecification(new SpanEquipmentSpecification(Multi_Ø50_12x7_5x10, "Conduit", "Ø50 5x10+12x7",
              new SpanStructureTemplate(Ø50_Orange, 1, 1,
                  new SpanStructureTemplate[] {
                        new SpanStructureTemplate(Ø10_Blue, 2, 1, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Yellow, 2, 2, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_White, 2, 3, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Green, 2, 4, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Black, 2, 5, Array.Empty<SpanStructureTemplate>()),

                        new SpanStructureTemplate(Ø7_Blue, 2, 6, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Yellow, 2, 7, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_White, 2, 8, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Green, 2, 9, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Black, 2, 10, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Red, 2, 11, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Orange, 2, 12, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Pink, 2, 13, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Silver, 2, 14, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Brown, 2, 15, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Turquoise, 2, 16, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Violet, 2, 17, Array.Empty<SpanStructureTemplate>())
                  })
              )
            {
                Description = "ø50 mm Multirør 5x10 + 10x7 standard farver",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true
            });

            AddSpecification(new SpanEquipmentSpecification(Multi_Ø50_12x7_5x10_BlueYellow, "Conduit", "Ø50 5x10+12x7",
             new SpanStructureTemplate(Ø50_Orange, 1, 1,
                 new SpanStructureTemplate[] {
                        new SpanStructureTemplate(Ø10_Blue, 2, 1, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Yellow, 2, 2, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_White, 2, 3, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Green, 2, 4, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Black, 2, 5, Array.Empty<SpanStructureTemplate>()),

                        new SpanStructureTemplate(Ø7_Blue, 2, 6, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Blue, 2, 7, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Blue, 2, 8, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Blue, 2, 9, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Blue, 2, 10, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Blue, 2, 11, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Yellow, 2, 12, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Yellow, 2, 13, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Yellow, 2, 14, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Yellow, 2, 15, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Yellow, 2, 16, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Yellow, 2, 17, Array.Empty<SpanStructureTemplate>())
                 })
             )
            {
                Description = "ø50 mm Multirør 5x10 + 12x7 Blue Yellow",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true
            });

            AddSpecification(new SpanEquipmentSpecification(Multi_Ø50_12x7_5x10_GreenWhite, "Conduit", "Ø50 5x10+12x7",
             new SpanStructureTemplate(Ø50_Orange, 1, 1,
                 new SpanStructureTemplate[] {
                        new SpanStructureTemplate(Ø10_Blue, 2, 1, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Yellow, 2, 2, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_White, 2, 3, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Green, 2, 4, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø10_Black, 2, 5, Array.Empty<SpanStructureTemplate>()),

                        new SpanStructureTemplate(Ø7_Green, 2, 6, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Green, 2, 7, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Green, 2, 8, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Green, 2, 9, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Green, 2, 10, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_Green, 2, 11, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_White, 2, 12, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_White, 2, 13, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_White, 2, 14, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_White, 2, 15, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_White, 2, 16, Array.Empty<SpanStructureTemplate>()),
                        new SpanStructureTemplate(Ø7_White, 2, 17, Array.Empty<SpanStructureTemplate>())
                 })
             )
            {
                Description = "ø50 mm Multirør 5x10 + 12x7 Green White",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true
            });


            // Tomrør
            AddSpecification(new SpanEquipmentSpecification(Flex_Ø40_Red, "Conduit", "Ø40 Flex", new SpanStructureTemplate(Ø40_Red, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø40 mm Flexrør",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = false,
                IsMultiLevel = true
            });

            AddSpecification(new SpanEquipmentSpecification(Tomrør_Ø40_Orange, "Conduit", "Ø40", new SpanStructureTemplate(Ø40_Orange, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø40 mm orange tomrør",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = false,
                IsMultiLevel = true
            });

            AddSpecification(new SpanEquipmentSpecification(Tomrør_Ø50_Orange, "Conduit", "Ø50", new SpanStructureTemplate(Ø50_Orange, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø50 mm orange tomrør",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = false,
                IsMultiLevel = true
            });

            AddSpecification(new SpanEquipmentSpecification(Tomrør_Ø110_Red, "Conduit", "Ø110", new SpanStructureTemplate(Ø110_Red, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø110 mm rød tomrør",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = false,
                IsMultiLevel = true
            });

            // Ø7 Enkelt rør
            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø7_Blue, "SingleConduit", "Ø7", new SpanStructureTemplate(Ø7_Blue, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø7/5 blå",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø7_Yellow, "SingleConduit", "Ø7", new SpanStructureTemplate(Ø7_Yellow, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø7/5 gul",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø7_White, "SingleConduit", "Ø7", new SpanStructureTemplate(Ø7_White, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø7/5 hvid",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø7_Green, "SingleConduit", "Ø7", new SpanStructureTemplate(Ø7_Green, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø7/5 grøn",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø7_Black, "SingleConduit", "Ø7", new SpanStructureTemplate(Ø7_Black, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø7/5 sort",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø7_Red, "SingleConduit", "Ø7", new SpanStructureTemplate(Ø7_Red, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø7/5 rød",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø7_Orange, "SingleConduit", "Ø7", new SpanStructureTemplate(Ø7_Orange, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø7/5 orange",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø7_Pink, "SingleConduit", "Ø7", new SpanStructureTemplate(Ø7_Pink, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø7/5 pink",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø7_Silver, "SingleConduit", "Ø7", new SpanStructureTemplate(Ø7_Silver, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø7/5 grå",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø7_Brown, "SingleConduit", "Ø7", new SpanStructureTemplate(Ø7_Brown, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø7/5 brun",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø7_Turquoise, "SingleConduit", "Ø7", new SpanStructureTemplate(Ø7_Turquoise, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø7/5 turkis",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø7_Violet, "SingleConduit", "Ø7", new SpanStructureTemplate(Ø7_Violet, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø7/5 violet",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });


            // Ø10 single conduit
            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø10_Blue, "SingleConduit", "Ø10", new SpanStructureTemplate(Ø10_Blue, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø10/8 blå",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø10_Yellow, "SingleConduit", "Ø10", new SpanStructureTemplate(Ø10_Yellow, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø10/8 gul",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø10_White, "SingleConduit", "Ø10", new SpanStructureTemplate(Ø10_White, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø10/8 hvid",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø10_Green, "SingleConduit", "Ø10", new SpanStructureTemplate(Ø10_Green, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø10/8 grøn",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø10_Black, "SingleConduit", "Ø10", new SpanStructureTemplate(Ø10_Black, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø10/8 sort",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø10_Red, "SingleConduit", "Ø10", new SpanStructureTemplate(Ø10_Red, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø10/8 rød",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø10_Orange, "SingleConduit", "Ø10", new SpanStructureTemplate(Ø10_Orange, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø10/8 orange",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø10_Pink, "SingleConduit", "Ø10", new SpanStructureTemplate(Ø10_Pink, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø10/8 pink",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø10_Silver, "SingleConduit", "Ø10", new SpanStructureTemplate(Ø10_Silver, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø10/8 grå",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø10_Brown, "SingleConduit", "Ø10", new SpanStructureTemplate(Ø10_Brown, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø10/8 brun",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø10_Turquoise, "SingleConduit", "Ø10", new SpanStructureTemplate(Ø10_Turquoise, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø10/8 turkis",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø10_Violet, "SingleConduit", "Ø10", new SpanStructureTemplate(Ø10_Violet, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø10/8 violet",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            // Ø12 single conduit
            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø12_Orange, "SingleConduit", "Ø12", new SpanStructureTemplate(Ø12_Orange, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø12/10 orange",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

            // Ø12 single conduit
            AddSpecification(new SpanEquipmentSpecification(SingleConduit_Ø12_Red, "SingleConduit", "Ø12", new SpanStructureTemplate(Ø12_Red, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø12/10 rød",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });


            // Ø12 customer conduit
            AddSpecification(new SpanEquipmentSpecification(CustomerConduit_Ø12_Orange, "CustomerConduit", "Ø12", new SpanStructureTemplate(Ø12_Orange, 1, 1, Array.Empty<SpanStructureTemplate>()))
            {
                Description = "ø12/8 orange",
                ManufacturerRefs = new Guid[] { Manu_GMPlast, Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = false
            });

        }

        private void AddSpanStructureSpecifications()
        {
            AddSpecification(new SpanStructureSpecification(Ø7_Blue, "Conduit", "Ø7/5", "Blue") { OuterDiameter = 7, InnerDiameter = 5 });
            AddSpecification(new SpanStructureSpecification(Ø7_Yellow, "Conduit", "Ø7/5", "Yellow") { OuterDiameter = 7, InnerDiameter = 5 });
            AddSpecification(new SpanStructureSpecification(Ø7_White, "Conduit", "Ø7/5", "White") { OuterDiameter = 7, InnerDiameter = 5 });
            AddSpecification(new SpanStructureSpecification(Ø7_Green, "Conduit", "Ø7/5", "Green") { OuterDiameter = 7, InnerDiameter = 5 });
            AddSpecification(new SpanStructureSpecification(Ø7_Black, "Conduit", "Ø7/5", "Black") { OuterDiameter = 7, InnerDiameter = 5 });
            AddSpecification(new SpanStructureSpecification(Ø7_Red, "Conduit", "Ø7/5", "Red") { OuterDiameter = 7, InnerDiameter = 5 });
            AddSpecification(new SpanStructureSpecification(Ø7_Orange, "Conduit", "Ø7/5", "Orange") { OuterDiameter = 7, InnerDiameter = 5 });
            AddSpecification(new SpanStructureSpecification(Ø7_Pink, "Conduit", "Ø7/5", "Pink") { OuterDiameter = 7, InnerDiameter = 5 });
            AddSpecification(new SpanStructureSpecification(Ø7_Silver, "Conduit", "Ø7/5", "Silver") { OuterDiameter = 7, InnerDiameter = 5 });
            AddSpecification(new SpanStructureSpecification(Ø7_Brown, "Conduit", "Ø7/5", "Brown") { OuterDiameter = 7, InnerDiameter = 5 });
            AddSpecification(new SpanStructureSpecification(Ø7_Turquoise, "Conduit", "Ø7/5", "Turquoise") { OuterDiameter = 7, InnerDiameter = 5 });
            AddSpecification(new SpanStructureSpecification(Ø7_Violet, "Conduit", "Ø7/5", "Violet") { OuterDiameter = 7, InnerDiameter = 5 });


            AddSpecification(new SpanStructureSpecification(Ø10_Blue, "Conduit", "Ø10/8", "Blue") { OuterDiameter = 10, InnerDiameter = 8 });
            AddSpecification(new SpanStructureSpecification(Ø10_Yellow, "Conduit", "Ø10/8", "Yellow") { OuterDiameter = 10, InnerDiameter = 8 });
            AddSpecification(new SpanStructureSpecification(Ø10_White, "Conduit", "Ø10/8", "White") { OuterDiameter = 10, InnerDiameter = 8 });
            AddSpecification(new SpanStructureSpecification(Ø10_Green, "Conduit", "Ø10/8", "Green") { OuterDiameter = 10, InnerDiameter = 8 });
            AddSpecification(new SpanStructureSpecification(Ø10_Black, "Conduit", "Ø10/8", "Black") { OuterDiameter = 10, InnerDiameter = 8 });
            AddSpecification(new SpanStructureSpecification(Ø10_Red, "Conduit", "Ø10/8", "Red") { OuterDiameter = 10, InnerDiameter = 8 });
            AddSpecification(new SpanStructureSpecification(Ø10_Orange, "Conduit", "Ø10/8", "Orange") { OuterDiameter = 10, InnerDiameter = 8 });
            AddSpecification(new SpanStructureSpecification(Ø10_Pink, "Conduit", "Ø10/8", "Pink") { OuterDiameter = 10, InnerDiameter = 8 });
            AddSpecification(new SpanStructureSpecification(Ø10_Silver, "Conduit", "Ø10/8", "Silver") { OuterDiameter = 10, InnerDiameter = 8 });
            AddSpecification(new SpanStructureSpecification(Ø10_Brown, "Conduit", "Ø10/8", "Brown") { OuterDiameter = 10, InnerDiameter = 8 });
            AddSpecification(new SpanStructureSpecification(Ø10_Turquoise, "Conduit", "Ø10/8", "Turquoise") { OuterDiameter = 10, InnerDiameter = 8 });
            AddSpecification(new SpanStructureSpecification(Ø10_Violet, "Conduit", "Ø10/8", "Violet") { OuterDiameter = 10, InnerDiameter = 8 });

            AddSpecification(new SpanStructureSpecification(Ø12_Orange, "Conduit", "Ø12/10", "Orange") { OuterDiameter = 12, InnerDiameter = 10 });
            AddSpecification(new SpanStructureSpecification(Ø12_Red, "Conduit", "Ø12/10", "Red") { OuterDiameter = 12, InnerDiameter = 10 });

            AddSpecification(new SpanStructureSpecification(Ø32_Orange, "Conduit", "Ø32", "Orange") { OuterDiameter = 32 });
            AddSpecification(new SpanStructureSpecification(Ø40_Orange, "Conduit", "Ø40", "Orange") { OuterDiameter = 40 });
            AddSpecification(new SpanStructureSpecification(Ø40_Red, "Conduit", "Ø40", "Red") { OuterDiameter = 40 });
            AddSpecification(new SpanStructureSpecification(Ø50_Orange, "Conduit", "Ø50", "Orange") { OuterDiameter = 50 });
            AddSpecification(new SpanStructureSpecification(Ø110_Red, "Conduit", "Ø110", "Red") { OuterDiameter = 110 });
        }

        private void AddFiberCableSpecifications()
        {
            AddSpecification(new SpanStructureSpecification(FiberCable_Jacket, "FiberCable", "Jacket", "Black") { OuterDiameter = 0, InnerDiameter = 0 });
            AddSpecification(new SpanStructureSpecification(FiberCable_Fiber, "FiberCable", "Fiber", "White") { OuterDiameter = 0, InnerDiameter = 0 });

            AddSpecification(new SpanEquipmentSpecification(FiberCable_2Fiber, "FiberCable", "2 Fiber", CreateFibers(2))
            {
                Description = "2 Fiber Cable",
                ManufacturerRefs = new Guid[] { Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true,
                IsCable = true
            });

            AddSpecification(new SpanEquipmentSpecification(FiberCable_4Fiber, "FiberCable", "4 Fiber", CreateFibers(4))
            {
                Description = "4 Fiber Cable",
                ManufacturerRefs = new Guid[] { Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true,
                IsCable = true
            });

            AddSpecification(new SpanEquipmentSpecification(FiberCable_12Fiber, "FiberCable", "12 Fiber", CreateFibers(12))
            {
                Description = "12 Fiber Cable",
                ManufacturerRefs = new Guid[] { Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true,
                IsCable = true
            });

            AddSpecification(new SpanEquipmentSpecification(FiberCable_24Fiber, "FiberCable", "24 Fiber", CreateFibers(24))
            {
                Description = "24 Fiber Cable",
                ManufacturerRefs = new Guid[] { Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true,
                IsCable = true
            });

            AddSpecification(new SpanEquipmentSpecification(FiberCable_48Fiber, "FiberCable", "48 Fiber", CreateFibers(48))
            {
                Description = "48 Fiber Cable",
                ManufacturerRefs = new Guid[] { Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true,
                IsCable = true
            });

            AddSpecification(new SpanEquipmentSpecification(FiberCable_72Fiber, "FiberCable", "72 Fiber", CreateFibers(72))
            {
                Description = "72 Fiber Cable",
                ManufacturerRefs = new Guid[] { Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true,
                IsCable = true
            });

            AddSpecification(new SpanEquipmentSpecification(FiberCable_96Fiber, "FiberCable", "96 Fiber", CreateFibers(96))
            {
                Description = "96 Fiber Cable",
                ManufacturerRefs = new Guid[] { Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true,
                IsCable = true
            });

            AddSpecification(new SpanEquipmentSpecification(FiberCable_144Fiber, "FiberCable", "144 Fiber", CreateFibers(144))
            {
                Description = "144 Fiber Cable",
                ManufacturerRefs = new Guid[] { Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true,
                IsCable = true
            });

            AddSpecification(new SpanEquipmentSpecification(FiberCable_192Fiber, "FiberCable", "192 Fiber", CreateFibers(192))
            {
                Description = "192 Fiber Cable",
                ManufacturerRefs = new Guid[] { Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true,
                IsCable = true
            });

            AddSpecification(new SpanEquipmentSpecification(FiberCable_216Fiber, "FiberCable", "216 Fiber", CreateFibers(216))
            {
                Description = "216 Fiber Cable",
                ManufacturerRefs = new Guid[] { Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true,
                IsCable = true
            });

            AddSpecification(new SpanEquipmentSpecification(FiberCable_288Fiber, "FiberCable", "288 Fiber", CreateFibers(288))
            {
                Description = "288 Fiber Cable",
                ManufacturerRefs = new Guid[] { Manu_Emtelle },
                IsFixed = true,
                IsMultiLevel = true,
                IsCable = true
            });
        }

        private SpanStructureTemplate CreateFibers(int numberOfFibers)
        {
            List<SpanStructureTemplate> fiberStructures = new();

            for (ushort fiberNo = 1; fiberNo <= numberOfFibers; fiberNo++)
            {
                fiberStructures.Add(new SpanStructureTemplate(FiberCable_Fiber, 2, fiberNo, Array.Empty<SpanStructureTemplate>()));
            }

            return new SpanStructureTemplate(FiberCable_Jacket, 1, 1, fiberStructures.ToArray());
        }



        private void AddManufactures()
        {
            // Manufacturer
            AddManufacturer(new Manufacturer(Manu_GMPlast, "GM Plast"));
            AddManufacturer(new Manufacturer(Manu_Emtelle, "Emtelle"));
            AddManufacturer(new Manufacturer(Manu_Fiberpowertech, "Fiberpowertech"));
            AddManufacturer(new Manufacturer(Manu_Cubis, "Wavin"));
            AddManufacturer(new Manufacturer(Manu_CommScope, "CommScope"));
            AddManufacturer(new Manufacturer(Manu_HuberSuhner, "Huber+Suhner"));
            AddManufacturer(new Manufacturer(Manu_Nokia, "Nokia"));
        }

        private void AddRackSpecifications()
        {
            // ESTI
            AddSpecification(new RackSpecification(Rack_ESTI, "CommScope ETSI Rack", "ETSI"));
            AddSpecification(new RackSpecification(Rack_Super, "Super rack", "Super rack"));
        }

        private void AddTerminalStructureSpecifications()
        {
            // 4 Pin Tray Uknown Type
            AddSpecification(new TerminalStructureSpecification(SpliceTray_Uknown4Pin, "SpliceTray", "Splidsebakke 4 Søm", "Bakke 4 Søm",
                new TerminalTemplate[]
                {
                    new TerminalTemplate("1", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("2", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("3", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("4", TerminalDirectionEnum.BI, false, true),
                }
            ));


            // 12 Pin Tray Uknown Type
            AddSpecification(new TerminalStructureSpecification(SpliceTray_Uknown12Pin, "SpliceTray", "Splidsebakke 12 Søm", "Bakke 12 Søm",
                new TerminalTemplate[]
                {
                    new TerminalTemplate("1", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("2", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("3", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("4", TerminalDirectionEnum.BI, false, true),   
                    new TerminalTemplate("5", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("6", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("7", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("8", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("9", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("10", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("11", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("12", TerminalDirectionEnum.BI, false, true)
                }
            ));

            // 24 Pin Tray Uknown Type
            AddSpecification(new TerminalStructureSpecification(SpliceTray_Uknown24Pin, "SpliceTray", "Splidsebakke 24 Søm", "Bakke 24 Søm",
                new TerminalTemplate[]
                {
                    new TerminalTemplate("1", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("2", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("3", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("4", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("5", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("6", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("7", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("8", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("9", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("10", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("11", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("12", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("13", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("14", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("15", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("16", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("17", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("18", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("19", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("20", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("21", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("22", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("23", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("24", TerminalDirectionEnum.BI, false, true)
                }
            ));

            // 12 Pin Tray SC (thin tray)
            AddSpecification(new TerminalStructureSpecification(SpliceTray_SC12Pin, "SpliceTray", "Small SC splidsebakke 12 Søm", "SC Bakke",
                new TerminalTemplate[]
                {
                    new TerminalTemplate("1", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("2", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("3", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("4", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("5", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("6", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("7", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("8", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("9", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("10", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("11", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("12", TerminalDirectionEnum.BI, false, true)
                }
            ));

            // 12 Pin Tray SE (thick tray)
            AddSpecification(new TerminalStructureSpecification(SpliceTray_SE12Pin, "SpliceTray", "Bred SE splidsebakke 12 Søm", "SE Bakke",
                new TerminalTemplate[]
                {
                    new TerminalTemplate("1", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("2", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("3", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("4", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("5", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("6", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("7", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("8", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("9", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("10", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("11", TerminalDirectionEnum.BI, false, true),
                    new TerminalTemplate("12", TerminalDirectionEnum.BI, false, true)
                }
            ));

            // LISA 24 Søm UPC+APC
            AddSpecification(new TerminalStructureSpecification(SplicePatchTray_LX12UPC12APC, "SplicePatchTray", "LISATray 12 x LX.5/UPC og 12 x LX.5/APC", "LISA UPC+APC",
                new TerminalTemplate[]
                {
                    new TerminalTemplate("1", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/APC"},
                    new TerminalTemplate("2", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/APC"},
                    new TerminalTemplate("3", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/APC"},
                    new TerminalTemplate("4", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/APC"},
                    new TerminalTemplate("5", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/APC"},
                    new TerminalTemplate("6", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/APC"},
                    new TerminalTemplate("7", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/UPC"},
                    new TerminalTemplate("8", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/UPC"},
                    new TerminalTemplate("9", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/UPC"},
                    new TerminalTemplate("10", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/UPC"},
                    new TerminalTemplate("11", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/UPC"},
                    new TerminalTemplate("12", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/UPC"},
                    new TerminalTemplate("13", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/APC"},
                    new TerminalTemplate("14", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/APC"},
                    new TerminalTemplate("15", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/APC"},
                    new TerminalTemplate("16", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/APC"},
                    new TerminalTemplate("17", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/APC"},
                    new TerminalTemplate("18", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/APC"},
                    new TerminalTemplate("19", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/UPC"},
                    new TerminalTemplate("20", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/UPC"},
                    new TerminalTemplate("21", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/UPC"},
                    new TerminalTemplate("22", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/UPC"},
                    new TerminalTemplate("23", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/UPC"},
                    new TerminalTemplate("24", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LX.5/UPC"}
                }
            ));


            // CommScope GPS2 12 SC
            AddSpecification(new TerminalStructureSpecification(SplicePatchTray_GPS2_12SC, "SplicePatchTray", "GPS2 12 x SC/APC", "GPS2 12xSC/APC",
                new TerminalTemplate[]
                {
                    new TerminalTemplate("1", TerminalDirectionEnum.BI, false, true) { ConnectorType = "SC/APC"},
                    new TerminalTemplate("2", TerminalDirectionEnum.BI, false, true) { ConnectorType = "SC/APC"},
                    new TerminalTemplate("3", TerminalDirectionEnum.BI, false, true) { ConnectorType = "SC/APC"},
                    new TerminalTemplate("4", TerminalDirectionEnum.BI, false, true) { ConnectorType = "SC/APC"},
                    new TerminalTemplate("5", TerminalDirectionEnum.BI, false, true) { ConnectorType = "SC/APC"},
                    new TerminalTemplate("6", TerminalDirectionEnum.BI, false, true) { ConnectorType = "SC/APC"},
                    new TerminalTemplate("7", TerminalDirectionEnum.BI, false, true) { ConnectorType = "SC/APC"},
                    new TerminalTemplate("8", TerminalDirectionEnum.BI, false, true) { ConnectorType = "SC/APC"},
                    new TerminalTemplate("9", TerminalDirectionEnum.BI, false, true) { ConnectorType = "SC/APC"},
                    new TerminalTemplate("10", TerminalDirectionEnum.BI, false, true) { ConnectorType = "SC/APC"},
                    new TerminalTemplate("11", TerminalDirectionEnum.BI, false, true) { ConnectorType = "SC/APC"},
                    new TerminalTemplate("12", TerminalDirectionEnum.BI, false, true) { ConnectorType = "SC/APC"},
                }
            ));

            // CommScope GPS2 24 
            AddSpecification(new TerminalStructureSpecification(SplicePatchTray_GPS2_24LC, "SplicePatchTray", "GPS2 24 x LC/APC", "GPS2 24xLC/APC",
                new TerminalTemplate[]
                {
                    new TerminalTemplate("1", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("2", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("3", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("4", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("5", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("6", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("7", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("8", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("9", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("10", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("11", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("12", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("13", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("14", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("15", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("16", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("17", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("18", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("19", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("20", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("21", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("22", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("23", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"},
                    new TerminalTemplate("24", TerminalDirectionEnum.BI, false, true) { ConnectorType = "LC/APC"}
                }
            ));

            // GSS 5 x 1:2 splitter bakke
            AddSpecification(new TerminalStructureSpecification(GSS_4x1_2_Splitter, "Splitters", "GSS Bakke med 4 stk 1:2 Splitter", "1:2 Split",
                new TerminalTemplate[]
                {
                    new TerminalTemplate("spl1_ind (søm 1)", TerminalDirectionEnum.IN, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl1" },
                    new TerminalTemplate("spl1_ud1 (søm 2)", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl1" },
                    new TerminalTemplate("spl1_ud2 (søm 3)", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl1" },
                    new TerminalTemplate("spl2_ind (søm 4)", TerminalDirectionEnum.IN, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl2" },
                    new TerminalTemplate("spl2_ud1 (søm 5)", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl2" },
                    new TerminalTemplate("spl2_ud2 (søm 6)", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl2" },
                    new TerminalTemplate("spl3_ind (søm 7)", TerminalDirectionEnum.IN, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl3" },
                    new TerminalTemplate("spl3_ud1 (søm 8)", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl3" },
                    new TerminalTemplate("spl3_ud2 (søm 9)", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl3" },
                    new TerminalTemplate("spl4_ind (søm 10)", TerminalDirectionEnum.IN, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl4" },
                    new TerminalTemplate("spl4_ud1 (søm 11)", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl4" },
                    new TerminalTemplate("spl4_ud2 (søm 12)", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl4" },
                 }
            ));

            // LISA 1:32 splitter
            AddSpecification(new TerminalStructureSpecification(LISA_1_32_Splitter, "Splitters", "Splittermodul PLC 1:32 4,6m LC/Apc LISA", "1:32 Split",
                new TerminalTemplate[]
                {
                    new TerminalTemplate("ind", TerminalDirectionEnum.IN, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud1", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud2", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud3", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud4", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud5", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud6", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud7", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud8", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud9", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud10", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud11", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud12", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud13", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud14", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud15", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud16", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud17", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud18", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud19", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud20", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud21", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud22", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud23", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud24", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud25", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud26", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud27", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud28", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud29", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud30", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud31", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" },
                    new TerminalTemplate("ud32", TerminalDirectionEnum.OUT, true, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl" }
                }
            ));

            // 8 Port Line Card
            AddSpecification(new TerminalStructureSpecification(OLT_LineCard8Port, "LineCards", "ISAM FD/FX 8port GPON Line board", "8 ports LT kort",
                new TerminalTemplate[]
                {
                    new TerminalTemplate("1", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("2", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("3", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("4", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("5", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("6", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("7", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("8", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" }
                }
            ));

            // 8 Port Line Card
            AddSpecification(new TerminalStructureSpecification(OLT_LineCard16Port, "LineCards", "ISAM FD/FX 16port GPON Line board", "16 ports LT kort",
                new TerminalTemplate[]
                {
                    new TerminalTemplate("1", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("2", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("3", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("4", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("5", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("6", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("7", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("8", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("9", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("10", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("11", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("12", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("13", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("14", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("15", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                    new TerminalTemplate("16", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" }
                }
            ));

            // Interface module
            AddSpecification(new TerminalStructureSpecification(OLT_InterfaceModule, "Interface", "Interface test module", "Interface test module",
                new TerminalTemplate[]
                {
                    new TerminalTemplate("1", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "SC/UPC", InternalConnectivityNode = "data" },
                }
            )
            {  
                IsInterfaceModule = true,
            }
            );


            // WDM Type 1
            AddSpecification(new TerminalStructureSpecification(LGX_WDMType1, "Couplers", "WDM Coupler CommScope Type 1 (LGX Modul)", "WDM Type 1",
                new TerminalTemplate[]
                {
                    new TerminalTemplate("RF AB", TerminalDirectionEnum.IN, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "tv" },
                    new TerminalTemplate("IP AD", TerminalDirectionEnum.IN, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl1" },
                    new TerminalTemplate("RF CD", TerminalDirectionEnum.IN, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "tv" },

                    new TerminalTemplate("RF EF", TerminalDirectionEnum.IN, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "tv" },
                    new TerminalTemplate("IP EH", TerminalDirectionEnum.IN, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl2" },
                    new TerminalTemplate("RF GH", TerminalDirectionEnum.IN, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "tv" },


                    new TerminalTemplate("COM A", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl1" },
                    new TerminalTemplate("COM B", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl1" },
                    new TerminalTemplate("COM C", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl1" },
                    new TerminalTemplate("COM D", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl1" },
                    new TerminalTemplate("COM E", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl2" },
                    new TerminalTemplate("COM F", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl2" },
                    new TerminalTemplate("COM G", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl2" },
                    new TerminalTemplate("COM H", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl2" }

                }
            ));

            // WDM Type 2
            AddSpecification(new TerminalStructureSpecification(LGX_WDMType2, "Couplers", "WDM Coupler CommScope Type 2 (LGX Modul)", "WDM Type 2",
                new TerminalTemplate[]
                {
                    new TerminalTemplate("RF A", TerminalDirectionEnum.IN, false, false) { ConnectorType = "LC/APC" },
                    new TerminalTemplate("IP AB", TerminalDirectionEnum.IN, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl1" },
                    new TerminalTemplate("RF B", TerminalDirectionEnum.IN, false, false) { ConnectorType = "LC/APC" },

                    new TerminalTemplate("RF C", TerminalDirectionEnum.IN, false, false) { ConnectorType = "LC/APC" },
                    new TerminalTemplate("IP CD", TerminalDirectionEnum.IN, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl2" },
                    new TerminalTemplate("RF D", TerminalDirectionEnum.IN, false, false) { ConnectorType = "LC/APC" },


                    new TerminalTemplate("COM A", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl1" },
                    new TerminalTemplate("COM B", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl1" },
                    new TerminalTemplate("COM C", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl2" },
                    new TerminalTemplate("COM D", TerminalDirectionEnum.OUT, false, false) { ConnectorType = "LC/APC", InternalConnectivityNode = "spl2" }
                
                }
            ));
        }

        private void AddTerminalEquipmentSpecifications()
        {
            // 12 Fiber Tray Uknown Type
            AddSpecification(new TerminalEquipmentSpecification(SpliceClosure_Uknown12Fiber, "SpliceClosure", "Ukendt Splidseboks 12 Fiber", "Splidseboks", false, 0,
                new TerminalStructureTemplate[]
                {
                }
            ));

            // 72 Fiber Tray Uknown Type
            AddSpecification(new TerminalEquipmentSpecification(SpliceClosure_Uknown72Fiber, "SpliceClosure", "Ukendt Splidseboks 72 Fiber", "Splidseboks", false, 0,
                new TerminalStructureTemplate[]
                {
                }
            ));


            // VMC 12 bakker
            AddSpecification(new TerminalEquipmentSpecification(SpliceClosure_VMC_12Tray, "SpliceClosure", "VMC Splidseboks 24 Fiber 12 stk. bakker med 12 søm", "VMC-12", false, 0,
                new TerminalStructureTemplate[]
                {
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 1),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 2),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 3),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 4),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 5),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 6),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 7),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 8),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 9),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 10),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 11),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 12)
                }
            ));

            // VMC 24 bakker
            AddSpecification(new TerminalEquipmentSpecification(SpliceClosure_VMC_24Tray, "SpliceClosure", "VMC Splidseboks 48 Fiber 24 stk. bakker med 12 søm", "VMC-24", false, 0,
                new TerminalStructureTemplate[]
                {
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 1),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 2),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 3),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 4),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 5),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 6),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 7),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 8),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 9),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 10),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 11),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 12),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 13),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 14),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 15),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 16),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 17),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 18),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 19),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 20),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 21),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 22),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 23),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 24)
                }
            ));

            // 3M 72 Fiber
            AddSpecification(new TerminalEquipmentSpecification(SpliceClosure_3M_72Fiber, "SpliceClosure", "3M Splidseboks 72 Fiber 6 stk. bakker med 12 søm", "3M", false, 0,
                new TerminalStructureTemplate[]
                {
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 1),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 2),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 3),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 4),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 5),
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 6)
                }
            ));

            // FIST Muffe
            AddSpecification(new TerminalEquipmentSpecification(SpliceClosure_FIST, "SpliceClosure", "FIST Splidsemuffe IP55 flad 1x24 UM 12 stk. SE bakker med 12 søm", "FIST Muffe", false, 0,
                new TerminalStructureTemplate[]
                {
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 1),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 2),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 3),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 4),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 5),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 6),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 7),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 8),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 9),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 10),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 11),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 12),
                }
            ));

            // FOSC400
            AddSpecification(new TerminalEquipmentSpecification(SpliceClosure_FOSC400, "SpliceClosure", "Splidsemuffe FOSC400 B4 3 stk bakker med 24 som", "FOSC400", false, 0,
                new TerminalStructureTemplate[]
                {
                    new TerminalStructureTemplate(SpliceTray_Uknown24Pin, 1),
                    new TerminalStructureTemplate(SpliceTray_Uknown24Pin, 2),
                    new TerminalStructureTemplate(SpliceTray_Uknown24Pin, 3),
                }
            ));


            // BUDI 1S 16 smal bakker
            AddSpecification(new TerminalEquipmentSpecification(SpliceClosure_BUDI1S_16SCTrays, "SpliceClosure", "BUDI-1S splidseboks 48 Fiber 16 stk. SC bakker med 12 søm", "BUDI-1S", false, 0,
                new TerminalStructureTemplate[]
                {
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 1),
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 2),
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 3),
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 4),
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 5),
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 6),
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 7),
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 8),
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 9),
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 10),
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 11),
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 12),
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 13),
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 14),
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 15),
                    new TerminalStructureTemplate(SpliceTray_SC12Pin, 16)
               }
            )
            { ManufacturerRefs = new Guid[] { Manu_CommScope } });

            // BUDI 1S 6 brede bakker
            AddSpecification(new TerminalEquipmentSpecification(SpliceClosure_BUDI1S_6SETrays, "SpliceClosure", "BUDI-1S splidseboks 72 Fiber 6 stk. SE bakker med 12 søm", "BUDI-1S", false, 0,
                new TerminalStructureTemplate[]
                {
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 1),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 2),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 3),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 4),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 5),
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 6)
               }
            )
            { ManufacturerRefs = new Guid[] { Manu_CommScope } });


            // BUDI 2S 1 bakke
            AddSpecification(new TerminalEquipmentSpecification(SpliceClosure_BUDI2S_1SETrays, "SpliceClosure", "BUDI-2S splidseboks 12 Fiber 1 stk. SE bakker med 12 søm", "BUDI-2S", false, 0,
                new TerminalStructureTemplate[]
                {
                    new TerminalStructureTemplate(SpliceTray_SE12Pin, 1)
               }
            )
            { ManufacturerRefs = new Guid[] { Manu_CommScope } });


            // FTUO
            AddSpecification(new TerminalEquipmentSpecification(SpliceClosure_FTUO, "SpliceClosure", "FTUO udvendig splideboks med plads til 12 søm", "FTUO", false, 0,
                new TerminalStructureTemplate[]
                {
                    new TerminalStructureTemplate(SpliceTray_Uknown12Pin, 1)
               }
            )
            { ManufacturerRefs = new Guid[] { Manu_CommScope } });

            // Customer termination
            AddSpecification(new TerminalEquipmentSpecification(CustomerTermination, "Kundeterminering", "Kundeterminering", "Kundeterminering med 4 søm", false, 0,
                new TerminalStructureTemplate[]
                {
                    new TerminalStructureTemplate(SpliceTray_Uknown4Pin, 1)
               }
            )
            {
                IsCustomerTermination = true
            });



            // LISA 24 Søm APC+UDF Tray
            AddSpecification(new TerminalEquipmentSpecification(Subrack_LISA_APC_UPC, "Subrack", "LISATray 12 x LX.5/UPC og 12 x LX.5/APC", "LISA APC+UPC", true, 1,
                new TerminalStructureTemplate[]
                {
                    new TerminalStructureTemplate(SplicePatchTray_LX12UPC12APC, 1)
               }
            )
            { ManufacturerRefs = new Guid[] { Manu_HuberSuhner } });


            // Patch/splidse GPS2 19" 72xSC/APC  
            AddSpecification(new TerminalEquipmentSpecification(Subrack_GPS_72_SC, "Subrack", "Patch/splidse GPS2 19\" 72 x SC/APC", "GPS2-72", true, 4,
                new TerminalStructureTemplate[]
                {
                    new TerminalStructureTemplate(SplicePatchTray_GPS2_12SC, 1),
                    new TerminalStructureTemplate(SplicePatchTray_GPS2_12SC, 2),
                    new TerminalStructureTemplate(SplicePatchTray_GPS2_12SC, 3),
                    new TerminalStructureTemplate(SplicePatchTray_GPS2_12SC, 4),
                    new TerminalStructureTemplate(SplicePatchTray_GPS2_12SC, 5),
                    new TerminalStructureTemplate(SplicePatchTray_GPS2_12SC, 6)
               }
            )
            { ManufacturerRefs = new Guid[] { Manu_CommScope } });


            // Patch/splidse GPS2 19" 144xLC/APC  
            AddSpecification(new TerminalEquipmentSpecification(Subrack_GPS_144_LC, "Subrack", "Patch/splidse GPS2 19\" 144 x LC/APC", "GPS2-144", true, 4,
                new TerminalStructureTemplate[]
                {
                    new TerminalStructureTemplate(SplicePatchTray_GPS2_24LC, 1),
                    new TerminalStructureTemplate(SplicePatchTray_GPS2_24LC, 2),
                    new TerminalStructureTemplate(SplicePatchTray_GPS2_24LC, 3),
                    new TerminalStructureTemplate(SplicePatchTray_GPS2_24LC, 4),
                    new TerminalStructureTemplate(SplicePatchTray_GPS2_24LC, 5),
                    new TerminalStructureTemplate(SplicePatchTray_GPS2_24LC, 6)
               }
            )
            { ManufacturerRefs = new Guid[] { Manu_CommScope } });



            // GSS with 24 1:2 splitters  
            AddSpecification(new TerminalEquipmentSpecification(GSS_24_Splitters, "Subrack", "GSS m. 1:2 split komplet", "1:2 Split", true, 4,
                new TerminalStructureTemplate[]
                {
                    new TerminalStructureTemplate(GSS_4x1_2_Splitter, 1),
                    new TerminalStructureTemplate(GSS_4x1_2_Splitter, 2),
                    new TerminalStructureTemplate(GSS_4x1_2_Splitter, 3),
                    new TerminalStructureTemplate(GSS_4x1_2_Splitter, 4),
                    new TerminalStructureTemplate(GSS_4x1_2_Splitter, 5),
                    new TerminalStructureTemplate(GSS_4x1_2_Splitter, 6),
                 }
            )
            { ManufacturerRefs = new Guid[] { Manu_CommScope } });

            // LISA splitter holder 
            AddSpecification(new TerminalEquipmentSpecification(LISA_SplitterHolder, "Subrack", "Splitterholder HuberSuhner f 6x5 splittere", "Splitterholder LISA", true, 2,
                Array.Empty<TerminalStructureTemplate>()
            )
            { ManufacturerRefs = new Guid[] { Manu_HuberSuhner } });

            // LGX Holder
            AddSpecification(new TerminalEquipmentSpecification(LGX_Holder, "Subrack", "Bæreramme for LGX", "LGX Holder", true, 4,
                Array.Empty<TerminalStructureTemplate>()
            ));

            // Nokia OLT
            AddSpecification(new TerminalEquipmentSpecification(OLT, "Subrack", "NFXS-E-7360", "NFXS-E-7360 ISAM FX-8 shelf", true, 8,
                Array.Empty<TerminalStructureTemplate>()
            )
            { ManufacturerRefs = new Guid[] { Manu_Nokia } });

        }


        private void AddSpecification(SpanEquipmentSpecification spec)
        {
            var cmd = new AddSpanEquipmentSpecification(Guid.NewGuid(), new UserContext("test", Guid.Empty), spec);
            var cmdResult = _commandDispatcher.HandleAsync<AddSpanEquipmentSpecification, Result>(cmd).Result;

            if (cmdResult.IsFailed)
                throw new ApplicationException(cmdResult.Errors.First().Message);
        }

        private void AddSpecification(SpanStructureSpecification spec)
        {
            var cmd = new AddSpanStructureSpecification(Guid.NewGuid(), new UserContext("test", Guid.Empty), spec);
            var cmdResult = _commandDispatcher.HandleAsync<AddSpanStructureSpecification, Result>(cmd).Result;

            if (cmdResult.IsFailed)
                throw new ApplicationException(cmdResult.Errors.First().Message);
        }

        private void AddSpecification(NodeContainerSpecification spec)
        {
            var cmd = new AddNodeContainerSpecification(Guid.NewGuid(), new UserContext("test", Guid.Empty), spec);
            var cmdResult = _commandDispatcher.HandleAsync<AddNodeContainerSpecification, Result>(cmd).Result;

            if (cmdResult.IsFailed)
                throw new ApplicationException(cmdResult.Errors.First().Message);
        }

        private void AddSpecification(RackSpecification spec)
        {
            var cmd = new AddRackSpecification(Guid.NewGuid(), new UserContext("test", Guid.Empty), spec);
            var cmdResult = _commandDispatcher.HandleAsync<AddRackSpecification, Result>(cmd).Result;

            if (cmdResult.IsFailed)
                throw new ApplicationException(cmdResult.Errors.First().Message);
        }

        private void AddManufacturer(Manufacturer manufacturer)
        {
            var cmd = new AddManufacturer(Guid.NewGuid(), new UserContext("test", Guid.Empty), manufacturer);
            var cmdResult = _commandDispatcher.HandleAsync<AddManufacturer, Result>(cmd).Result;

            if (cmdResult.IsFailed)
                throw new ApplicationException(cmdResult.Errors.First().Message);
        }

        private void AddSpecification(TerminalEquipmentSpecification spec)
        {
            var cmd = new AddTerminalEquipmentSpecification(Guid.NewGuid(), new UserContext("test", Guid.Empty), spec);
            var cmdResult = _commandDispatcher.HandleAsync<AddTerminalEquipmentSpecification, Result>(cmd).Result;

            if (cmdResult.IsFailed)
                throw new ApplicationException(cmdResult.Errors.First().Message);
        }

        private void AddSpecification(TerminalStructureSpecification spec)
        {
            var cmd = new AddTerminalStructureSpecification(Guid.NewGuid(), new UserContext("test", Guid.Empty), spec);
            var cmdResult = _commandDispatcher.HandleAsync<AddTerminalStructureSpecification, Result>(cmd).Result;

            if (cmdResult.IsFailed)
                throw new ApplicationException(cmdResult.Errors.First().Message);
        }
    }
}

