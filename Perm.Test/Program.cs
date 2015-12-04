using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Manage.Service.MongoDb.SQLAnalyzer;

namespace Perm.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var sql = @"select  a,c,dd FROM t1 where a like '%ss%' and b=1 or c!='3' or d in('1',2,'A') or e not like '%s' 
and f>5 and g<='x' and h>100 or i>=200 and j between 1 and 100 or k not in(3,4) and  L like 'vvv%' 
Order by Id desc,ss asc offset 20 ROWS
FETCH next 10 ROWS ONLY";
            TSQLTranslator.TranslateQuery(sql);

            var sql2 = @"select  a,c,dd FROM t1 where a like '%ss%' and b=1 or c!='3' or d in('1',2,'A') or e not like '%s' 
and f>5 and g<='x' and h>100 or i>=200 and j between 1 and 100 or k not in(3,4) and  L like 'vvv%' 
Order by Id desc,ss asc offset 20 ROWS
FETCH next {0} ROWS ONLY";

            var q = (from item in Enumerable.Range(1, 1 << 12)
                    select string.Format(sql2, item)).ToList();

            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1 << 12; i++)
                TSQLTranslator.TranslateQuery(q[i]);
            sw.Stop();
            Console.WriteLine("process at " + (decimal)(1 << 12) / sw.ElapsedMilliseconds * 1000M+"/s");
            Console.ReadLine();
        }
    }
}
