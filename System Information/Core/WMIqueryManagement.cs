using System.Management;

namespace System_Information.Core
{
    public class WmIqueryManagement
    {
        public WmIqueryReturn WmIquery(string classes, string[] properties, string conditionPropertyNotNull = "")
        {
            //WMIqueryReturn Object properties
            var nbResult = 0;

            // string of all properties elements
            var propertiesQuery = string.Join(", ", properties);

            // searcher object
            ManagementObjectSearcher searcher;
            if (conditionPropertyNotNull.Length > 1)
            {
                searcher = new ManagementObjectSearcher("root\\CIMV2",
                    $"SELECT {propertiesQuery} FROM {classes} WHERE {conditionPropertyNotNull} IS NOT NULL");
            }
            else
            {
                searcher = new ManagementObjectSearcher("root\\CIMV2", $"SELECT {propertiesQuery} FROM {classes}");
            }


            var propertiesResultList = new object[searcher.Get().Count, properties.Length];
            foreach (var queryObj in searcher.Get())
            {
                int index = 0;
                foreach (var property in properties)
                {
                    propertiesResultList[nbResult, index] = (queryObj[property]);
                    index++;
                }
                nbResult++;
            }

            // return the WMIqueryReturn Object with all properties
            return new WmIqueryReturn(nbResult, propertiesResultList);
        }
    }

    public class WmIqueryReturn
    {
        public int NbResult { get; set; }
        public object[,] PropertiesResultList { get; set; }

        public WmIqueryReturn( int nbResult, object[,] propertiesResultList)
        {
            NbResult = nbResult;
            PropertiesResultList = propertiesResultList;
        }
    }
}
