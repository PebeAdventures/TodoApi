using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Mapping;

namespace TodoApi.Tests.Mapping;
public class MappingProfileTests
{
    [Fact]
    public void Configuration_is_valid()
    {
        var cfg = new MapperConfiguration(c => c.AddProfile<MappingProfile>());
        cfg.AssertConfigurationIsValid();
    }
}
