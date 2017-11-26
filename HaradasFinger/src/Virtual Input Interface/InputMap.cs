using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Tekken7 {
    class InputMap {

        public static void LoadInputMap(string filePath, out Dictionary<string, InputItem> inputDict, out string mapName) {
            inputDict = new Dictionary<string, InputItem>();
            string inputDescription;
            InputItem inputObj;

            XmlDocument inputMap =  new XmlDocument();
            try {
                inputMap.Load(filePath);
            } catch (Exception ex) {
                Console.WriteLine("Error opening input map file {0}", filePath);
                throw ex;
            }

            XmlNodeList mapNode = inputMap.GetElementsByTagName("MapName");
            mapName = mapNode.Item(0).InnerText;

            XmlNodeList nodeList = inputMap.GetElementsByTagName("Input");
            foreach (XmlNode node in nodeList) {
                inputObj = CreateInputItem(node, out inputDescription);
                if (inputObj != null && !inputDict.ContainsValue(inputObj) && !inputDict.ContainsKey(inputObj.Name)) {
                    inputDict.Add(inputDescription, inputObj);
                } else {
                    Console.WriteLine("Could not add input {0}, please check {1} for duplicate or invalid entries", inputDescription, filePath);
                    //TODO: Throw an exception?
                }
            }
        }

        private static InputItem CreateInputItem(XmlNode inputNode, out string inputDescription) {
            uint inputIndex = 0;
            string inputType = "";
            XmlNode tempNode;
            InputItem retObj = null;

            inputDescription = "";
            tempNode = inputNode.FirstChild;

            while (tempNode != null) {
                if (tempNode.Name == "Position") {
                    inputIndex = (uint)UInt32.Parse(tempNode.InnerText);
                } else if (tempNode.Name == "Description") {
                    inputDescription = tempNode.InnerText.ToUpper();
                } else if (tempNode.Name == "InputType") {
                    inputType = tempNode.InnerText.ToUpper();
                }
                tempNode = tempNode.NextSibling;
            }

            if (inputType == "BUTTON") {
                retObj = new Button(inputDescription, inputIndex);
            } else if (inputType == "DIRECTION") {
                retObj = new Direction(inputDescription, inputIndex);
            } else { //just make a button for now if its ambiguous, not sure what to do here
                retObj = new Button(inputDescription, inputIndex);
            }

            return retObj;
        }
    }
}
