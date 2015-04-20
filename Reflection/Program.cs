using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflection
{
    public class Program
    {
        static void Main(string[] args)
        {
            //现在有对象Person 里面有很多字段，要想把对象List<Person> persons 的所有数据复制到 List<ViewPersonTwo> views
            //就要用到反射
            List<Person> persons = new List<Person>() { 
              new Person{Name="张三",Age=23,Height="123",Address="马路1号",Phone="1234432"},
              new Person{Name="小明",Age=23,Height="221",Address="马路2号",Phone="4433"},
              new Person{Name="李四",Age=23,Height="111",Address="马路3号",Phone="5555"},
              new Person{Name="王五",Age=23,Height="321",Address="马路4号",Phone="55666"},
              new Person{Name="赵天",Age=23,Height="223",Address="马路5号",Phone="7766"},
              new Person{Name="刘伟",Age=23,Height="222",Address="马路6号",Phone="1234432"}
            };
            List<ViewPersonTwo> views = new List<ViewPersonTwo>();

            //两个字段可以放在xml文件里，进行灵活配置
            //要展现的列
            string fields = "Name,Age,Height,Address,Phone";
            //目标列(target的字段应大于或等于fields的字段)
            string target = "A,B,C,D,E";


            views = ConvertToViewPerson(persons,fields,target);
            foreach (var entity in views)
            {
                Console.WriteLine(string.Format("{0},{1},{2},{3},{4}",entity.A,entity.B,entity.C,entity.D,entity.E));
            }
            Console.ReadKey();
        }
        /// <summary>
        /// 转换方法
        /// </summary>
        /// <param name="entity">Person数据</param>
        /// <param name="fields">需要转换，对应的字段. 自定义要显示的字段:"Name,Age";  </param>
        /// <returns></returns>
        public static List<ViewPersonTwo> ConvertToViewPerson(List<Person> entity, string fields,string target)
        {
            List<ViewPersonTwo> views = new List<ViewPersonTwo>();
            string[] str = fields.Split(',');
            foreach (var signal in entity)
            {
                string value = "";
                for (int i = 0; i < str.Length; i++)
                {
                    //通过反射获取值
                    value += Convert.ToString(signal.GetType().GetProperty(str[i]).GetValue(signal, null)) + ",";
                }
                views.Add(SignalData(value.TrimEnd(','), target));
            }
            return views;
 
        }
        /// <summary>
        /// 对 ViewPersonTwo 对象进项赋值
        /// </summary>
        /// <param name="value">原对象的值</param>
        /// <param name="target">目标对象的字段</param>
        /// <returns></returns>
        public static ViewPersonTwo SignalData(string value,string target)
        {
            ViewPersonTwo views = new ViewPersonTwo();
            string[] originalStr = value.Split(',');          
            string[] targetStr = target.Split(',');
            for (int i = 0; i < originalStr.Length; i++)
            {
                //通过反射设置值
                views.GetType().GetProperty(targetStr[i]).SetValue(views, originalStr[i], null);
            }
            return views;
        }
    }
}
