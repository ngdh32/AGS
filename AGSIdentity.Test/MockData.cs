using System;
using System.Collections.Generic;
using AGSIdentity.Models.EntityModels.AGSIdentity;

namespace AGSIdentity.Test
{
    public static class MockData
    {
        public static List<AGSFunctionClaimEntity> functionClaims = new List<AGSFunctionClaimEntity>()
            {
                new AGSFunctionClaimEntity()
                {
                    Id = "1",
                    Name = "function_1"
                },
                new AGSFunctionClaimEntity()
                {
                    Id = "2",
                    Name = "function_2"
                },
                new AGSFunctionClaimEntity()
                {
                    Id = "3",
                    Name = "function_3"
                }
            };

        public static List<AGSUserEntity> users = new List<AGSUserEntity>()
        {
            new AGSUserEntity()
            {
                Id = "1",
                Username = CommonConstant.AGSAdminName,
                GroupIds = new List<string>()
                {
                    "1",
                    "2"
                }
            },
            new AGSUserEntity()
            {
                Id = "2",
                Username = "Username2",
                GroupIds = new List<string>()
                {
                    "1",
                    "2"
                }
            },
            new AGSUserEntity()
            {
                Id = "3",
                Username = "Username3",
                GroupIds = new List<string>()
                {
                    "1",
                    "2"
                }
            }
        };

        public static List<AGSGroupEntity> groups = new List<AGSGroupEntity>()
        {
            new AGSGroupEntity()
            {
                Id = "1",
                Name = "Group_1",
                FunctionClaimIds = new List<string>()
                {
                    "1","2","3"
                }
            },
            new AGSGroupEntity()
            {
                Id = "2",
                Name = "Group_2",
                FunctionClaimIds = new List<string>()
                {
                    "1","2","3"
                }
            },
            new AGSGroupEntity()
            {
                Id = "3",
                Name = "Group_3",
                FunctionClaimIds = new List<string>()
                {
                    "1","2","3"
                }
            }
        };

    }
}
