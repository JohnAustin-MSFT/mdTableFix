using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization.NamingConventions;
using System.Text;

namespace YamlDotNet.Samples
{
    public class DeserializeObjectGraph
    {

        public void anotherTry()
        {
            // Setup the input
            var input = new StringReader(Content2);
            

            // Load the stream
            var yaml = new YamlStream();
            yaml.Load(input);

            // Examine the stream
            var mapping =
                (YamlMappingNode)yaml.Documents[0].RootNode;

            //var yList = mapping.Children;
            //if (yList.ContainsKey("title"))

            foreach (var entry in mapping.Children)
            {
                Console.WriteLine(((YamlScalarNode)entry.Key).Value);
                
            }

            // List all the items
            //var items = (YamlSequenceNode)mapping.Children[new YamlScalarNode("items")];
            //foreach (YamlMappingNode item in items)
            //{
            //    Console.WriteLine(
            //        "{0}\t{1}",
            //        item.Children[new YamlScalarNode("part_no")],
            //        item.Children[new YamlScalarNode("descrip")]
            //    );
            //}

            using (TextWriter writer = File.CreateText("e:\\newyaml.md"))
            {
                yaml.Save(writer);
                
            }
        }

        public void EntryPoint()
        {
            var input = new StringReader(Content);

            var deserializer = new DeserializerBuilder()
               // .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            var results = deserializer.Deserialize<Topic>(input);

                Console.WriteLine(results.title);

            //var order = deserializer.Deserialize<Order>(input);

            //Console.WriteLine("Order");
            //Console.WriteLine("-----");
            //Console.WriteLine();
            //foreach (var item in order.Items)
            //{
            //    Console.WriteLine("{0}\t{1}\t{2}\t{3}", item.PartNo, item.Quantity, item.Price, item.Descrip);
            //}
            //Console.WriteLine();

            //Console.WriteLine("Shipping");
            //Console.WriteLine("--------");
            //Console.WriteLine();
            //Console.WriteLine(order.ShipTo.Street);
            //Console.WriteLine(order.ShipTo.City);
            //Console.WriteLine(order.ShipTo.State);
            //Console.WriteLine();

            //Console.WriteLine("Billing");
            //Console.WriteLine("-------");
            //Console.WriteLine();
            //if (order.BillTo == order.ShipTo)
            //{
            //    Console.WriteLine("*same as shipping address*");
            //}
            //else
            //{
            //    Console.WriteLine(order.ShipTo.Street);
            //    Console.WriteLine(order.ShipTo.City);
            //    Console.WriteLine(order.ShipTo.State);
            //}
            //Console.WriteLine();

            //Console.WriteLine("Delivery instructions");
            //Console.WriteLine("---------------------");
            //Console.WriteLine();
            //Console.WriteLine(order.SpecialDelivery);
        }

        public class Order
        {
            public string Receipt { get; set; }
            public DateTime Date { get; set; }
            public Customer Customer { get; set; }
            public List<OrderItem> Items { get; set; }

            [YamlMember(Alias = "bill-to", ApplyNamingConventions = false)]
            public Address BillTo { get; set; }

            [YamlMember(Alias = "ship-to", ApplyNamingConventions = false)]
            public Address ShipTo { get; set; }

            public string SpecialDelivery { get; set; }
        }
        public class Topic
        {
            public string title { get; set; }
            public string msprod { get; set; }
            public string msassetid { get; set; }
        }

        public class Customer
        {
            public string Given { get; set; }
            public string Family { get; set; }
        }

        public class OrderItem
        {
            [YamlMember(Alias = "part_no", ApplyNamingConventions = false)]
            public string PartNo { get; set; }
            public string Descrip { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }

        public class Address
        {
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
        }

        private const string Content2 = @"---
title: ""Line 'item1': Maximum nesting level for controls exceeded with 'item2'.""
---";

        private const string Content = @"---
title: SharePoint Application Lifecycle Management
msprod: SHAREPOINT
msassetid: caaf9a09-2e6a-49e3-a8d6-aaf7f93a842a
";

        private const string Document = @"---
            receipt:    Oz-Ware Purchase Invoice
            date:        2007-08-06
            customer:
                given:   Dorothy
                family:  Gale

            items:
                - part_no:   A4786
                  descrip:   Water Bucket (Filled)
                  price:     1.47
                  quantity:  4

                - part_no:   E1628
                  descrip:   High Heeled ""Ruby"" Slippers
                  price:     100.27
                  quantity:  1

            bill-to:  &id001
                street: |-
                        123 Tornado Alley
                        Suite 16
                city:   East Westville
                state:  KS

            ship-to:  *id001

            specialDelivery: >
                Follow the Yellow Brick
                Road to the Emerald City.
                Pay no attention to the
                man behind the curtain.
---";
    }
}