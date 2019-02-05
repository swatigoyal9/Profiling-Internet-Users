using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;


namespace project

{
	class Class2
	{
		string path;
		_Application excel1 = new Microsoft.Office.Interop.Excel.Application();
		public Workbook workbook;
		public Worksheet worksheet;

		public Class2(string path, int sheet)
		{
			this.path = path;
			workbook = excel1.Workbooks.Open(path);
			worksheet = workbook.Worksheets[sheet];
		}

		
		//reading excel
		public double Reading1(int rn, int cn)
		{
			if (worksheet.Cells[rn, cn].Value2 != null)
				return worksheet.Cells[rn, cn].Value2;
			else
				return -1;

		}
		//reading first row
		public string FirstRow(int rn, int cn)
		{

			if (worksheet.Cells[rn, cn].Value2 != null)
				return worksheet.Cells[rn, cn].Value2;
			else
				return "nothing";
		}


	}
}
