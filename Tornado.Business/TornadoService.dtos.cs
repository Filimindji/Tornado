/* Options:
Date: 2015-06-13 22:09:03
Version: 1
BaseUrl: http://tornado-west-eu.cloudapp.net

//GlobalNamespace: 
//MakePartial: True
//MakeVirtual: True
//MakeDataContractsExtensible: False
//AddReturnMarker: True
//AddDescriptionAsComments: True
//AddDataContractAttributes: False
//AddIndexesToDataMembers: False
//AddResponseStatus: False
//AddImplicitVersion: 
//InitializeCollections: True
//IncludeTypes: 
//ExcludeTypes: 
//AddDefaultXmlNamespace: http://schemas.servicestack.net/types
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.DataAnnotations;
using Tornado.Server.ServiceModel;


namespace Tornado.Server.ServiceModel
{

    [Route("/file/{Hash}")]
    public partial class File
        : IReturn<FileResponse>
    {
        public virtual string Hash { get; set; }
    }

    public partial class FileResponse
    {
        public FileResponse()
        {
            Metadata = new Metadata[]{};
        }

        public virtual Metadata[] Metadata { get; set; }
    }

    [Route("/hello/{Name}")]
    public partial class Hello
        : IReturn<HelloResponse>
    {
        public virtual string Name { get; set; }
    }

    public partial class HelloResponse
    {
        public virtual string Result { get; set; }
    }

    public partial class Metadata
    {
        public virtual string Key { get; set; }
        public virtual string Value { get; set; }
    }
}

