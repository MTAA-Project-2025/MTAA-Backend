using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Fixtures
{
    [CollectionDefinition("Fixture collection")]
    public class SharedCollection : ICollectionFixture<ApiFixture>
    {
    }
}
