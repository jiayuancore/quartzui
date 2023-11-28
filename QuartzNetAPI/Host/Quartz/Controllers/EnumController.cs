using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Quartz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnumController : ControllerBase
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="enumKey"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EnumMemberResp>> Get([FromQuery] string enumKey)
        {
            var enumList = new List<EnumMemberResp>();
            Type[] types = Assembly
                   .Load("Host")
                   .GetTypes().Where(it => it.FullName.Contains("Host.Common.Enums"))
                   .ToArray();

            //枚举统一使用int
            foreach (Type type in types)
            {
                if (type.IsEnum)
                {
                    Array enums = System.Enum.GetValues(type);
                    foreach (var item in enums)
                    {
                        var en = new EnumMemberResp();

                        string member = item.ToString();  //  枚举成员 OPC 
                        int value = (int)item;    // 枚举数值 1

                        en.EnumKey = type.Name;
                        if (type.IsDefined(typeof(DescriptionAttribute), true))
                        {
                            var attribute = type.GetCustomAttribute(typeof(DescriptionAttribute), false);
                            en.EnumDescription = ((DescriptionAttribute)attribute).Description;
                        }

                        en.Value = value;
                        en.Name = member;
                        en.Description = member;

                        //获取中文描述需要用到MemberInfo/DescriptionAttribute
                        MemberInfo[] memInfo = type.GetMember(member);
                        object[] descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptionAttributes.Length > 0)
                        {
                            //中文描述
                            string desc = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                            en.Description = desc;
                        }
                        enumList.Add(en);
                    }
                }
            }
            var res = enumList.Where(p => p.EnumKey.Contains(enumKey ??= "")).ToList();

            return res;
        }
    }

    public class EnumMemberResp
    {
        /// <summary>
        /// 枚举
        /// </summary>
        public string EnumKey { get; set; }
        /// <summary>
        /// 枚举名称（描述）
        /// </summary>
        public string EnumDescription { get; set; }
        /// <summary>
        /// 成员值
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 成员名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 成员描述
        /// </summary>
        public string Description { get; set; }

    }
}
