using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;//.Tasks;

namespace TESTERC { 
class Program 
{
    public static string comp;

    public static int MakeChoise(string comp)//Выбор компилятора
    {
        int choise = 0;
        switch (comp)
        {
            case "C#":
                choise = 1;
                break;
            case "FreePascal":
                choise = 2;
                break;
            case "TurboPascal":
                choise = 3;
                break;
            case "Delphi":
                choise = 4;
                break;
            default:
                Console.WriteLine("Помилка у конфігураційному файлі");
                break;
        }

        return choise;//Возврат значения
    }
    static void Main(string[] args)
    {



        Console.WriteLine(@"              Програма Tester");



    //Введiть назву перевiряємої програми. Приклад:kalkf.pas
    //string file = Console.ReadLine();//Ввод имени и расширения файла программі
    nac:
        Console.WriteLine("Введiть назву конфiгурацiйного файлу Приклад: config.txt");
        string filetext = Console.ReadLine();
        string[] readText = File.ReadAllLines(filetext);


        Console.WriteLine("Введiть назву файлу куди зберiгати результати перевiрки.Приклад:restest.txt ");
        string path = Console.ReadLine();
        for (int a = 0; a < readText.Length; a++)
        {
                comp = null;
            string[] dirs = Directory.GetFiles(Directory.GetCurrentDirectory(), readText[a], SearchOption.AllDirectories);
            string[] info = Directory.GetFiles(Directory.GetCurrentDirectory(), readText[a + 1], SearchOption.AllDirectories);
            comp = readText[a + 2];
            for (int i = 0; i < dirs.Length; i++)
                {
                    
                    Compilation csc = new Compilation(dirs[i]);//Происходит компилирование файла
                   
                if (csc.Errors.IndexOf("error") != -1)//Если при компиляции возникли ошибки,выводит их на экран
                {
                    Console.WriteLine(csc.Errors);
                        Console.WriteLine(MakeChoise(comp));
                        using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine();
                        sw.WriteLine(info[0]);
                        sw.WriteLine(dirs[i]);
                        sw.WriteLine(comp);
                        sw.WriteLine("Compilation error");
                        sw.WriteLine();
                    }
                }
                else //Методу Test передается файл для проверки программы,при условии того что в программме нет ошибок
                {
                    Test test = new Test(info[0]);
                    Console.WriteLine(dirs[i]);
                    Console.WriteLine(test.Start(dirs[i])); //Программа проверяется по параметрам в файлу test(info)
                      

                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine();
                        sw.WriteLine(info[0]);
                        sw.WriteLine(dirs[i]);
                        sw.WriteLine(comp);
                        sw.WriteLine(test.Start(dirs[i]));
                        sw.WriteLine();
                    }
                        comp = null;
                }


            }
            if (a == readText.Length - 1)
            {
                goto ch;
            }
            a = a + 3;
        }
    ch:
        Console.WriteLine("Продовжити перевiрку чи завершити програму? [1] Продовжити  [2] Завершити");
        int choise = int.Parse(Console.ReadLine());
        switch (choise)
        {
            case 1:
                goto nac;
                break;
            case 2:
                goto kon;
                break;
            default:
                Console.WriteLine("Неправильный выбор");
                goto ch;
                break;



        }
    kon:
        Console.WriteLine("Для продовженния натиснить будь-яку кнопку");
        Console.ReadLine();

    }
}

public class Compilation//Метод компиляции программы
{
    Program MakeChoise = new Program();//Подключение метода MakeChoise
    public static int choise = Program.MakeChoise(Program.comp);

    public static FileInfo code_fil;

    //private FileInfo fpc_file = new FileInfo(@"Freepas\bin\Freepas\fpc.exe");//Путь файла компилятора FreePascalC:\FPC\3.0.0\bin\i386-win32\fpc.exe
    //private FileInfo csc_file = new FileInfo(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe ");//Путь файла компилятора C#
    private FileInfo csc_file = ChoiseCompilator(choise);

    private string errors;//Переменная куда записываются ошибки компилятора

    /// <summary>
    /// Компиляция кода си шарп
    /// Путь компилятора по умолчанию "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe "
    /// </summary>
    /// <param name="code_file">Файл для компиляции</param>
    public Compilation(string code_file)//Метод для определения ошибок
    {
        code_fil = new FileInfo(code_file);//Подключение файла(без этой строки программа роботать не будет)
        errors = "";
    }
    public static FileInfo ChoiseCompilator(int choise)
    {
        FileInfo csc_file;
        switch (choise)
        {
            case 1:
                csc_file = new FileInfo(@"C#\csc.exe");
                break;
            case 2:
                csc_file = new FileInfo(@"FreePas\bin\i386-win32\fpc.exe ");
                break;
            case 3:
                csc_file = new FileInfo(@"FreePas\bin\i386-win32\fpc.exe");
                break;
            case 4:
                csc_file = new FileInfo(@"FreePas\bin\i386-win32\fpc.exe");
                break;
            default: throw new FileNotFoundException("Неправильный выбор компилятора");
        }
        return csc_file;

    }
    public static string ChooseParametres()
    {

        string code_file1;
        switch (choise)
        {
            case 1:
                code_file1 = code_fil.ToString();
                break;
            case 2:
                code_file1 = code_fil.ToString();
                break;
            case 3:
                code_file1 = code_fil.ToString();
                code_file1 += " -Mtp";

                break;
            case 4:
                code_file1 = code_fil.ToString();
                code_file1 += " -Mdelphi";
                break;
            default: throw new NotFiniteNumberException("Ошибка при выборе параметра для компилятора!");
        }
        return code_file1;
    }
    /// <summary>
    /// Компиляция кода си шарп
    /// </summary>
    /// <param name="code_file">Файл для компиляции</param>
    /// <param name="csc_file">расположение компилятора</param>
    /*public Compilation(string code_file, string csc_file)
        : this(code_file)
    {

        switch(choise)
        {
            case 1:                this.csc_file = new FileInfo(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe ");
              break;
            case 2:                this.csc_file = new FileInfo(@"Freepas\bin\Freepas\fpc.exe");
                break;
            default: throw new FileNotFoundException("Неправильный выбор компилятора", this.code_file.FullName);
        }
        //this.fpc_file = new FileInfo(csc_file);
     }
    */


    // начать непостредственную компиляцию
    private void Start()
    {

        // если файла нет - выбросить исключение
        if (!code_fil.Exists)
        {
            throw new FileNotFoundException("Не удалось найти файл", code_fil.FullName);

        }

        // если нет файла компилятора
        if (!this.csc_file.Exists)
        {
            throw new FileNotFoundException("Не удалось найти файл компилятора", this.csc_file.FullName);
        }
        ProcessStartInfo csc;//Параметры запуска компиялтора
        Process tmp;//Позволяеьт управлять файлами в системе
        string code_file1 = ChooseParametres();

        csc = new ProcessStartInfo(csc_file.FullName, /*code_file.ToString*/code_file1);//Указывает имя файла и параметры запуска
        csc.UseShellExecute = false;//Нужно ли использовать оболочку windows
        csc.RedirectStandardOutput = true;//Записывать ли результаты компиляции


        tmp = Process.Start(csc);//Запускает саму компиляцию файла
        this.errors = tmp.StandardOutput.ReadToEnd();//Считывает ошибки и записывает их в переменную
    }

    /// <summary>
    /// Ошибки, которые выдал компилятор
    /// </summary>
    /// <exception cref="FileNotFoundException">Не удалось найти файл</exception>
    /// <exception cref="FileNotFoundException">Не удалось найти файл компилятора</exception>
    public string Errors
    {
        get
        {
            this.Start();
            return this.errors;
        }
    }

    /// <summary>
    /// Путь к компилятору
    /// </summary>
    public string CscFile//Путь к копмилятору
    {
        get { return this.csc_file.FullName; }//Берет имя
        set { this.csc_file = new FileInfo(value); }//Записывает это имя
    }
}

public class Test//Тестирование программы
{
    protected TimeSpan max_time; //Время на выполнение
    protected List<string> in_value = new List<string>();//Входящее значение
    protected List<string> out_value = new List<string>();//Выходящее значение
    protected string file;

    /// <summary>
    /// Информация о тестах
    /// </summary>
    /// <param name="file">Путь к файлу с интруциями</param>
    public Test(string file)//
    {
        this.file = file;
        this.FileAnalysis();
    }

    private void FileAnalysis()//Сам анализ файла
    {
        string result = "";//Результат
        bool is_path = true;//Есть путь или нет

        if (!File.Exists(this.file))//Если файл отсутствует
        {
            throw new FileNotFoundException("Файл конфигураци не найден", this.file);
        }
        StreamReader sr = new StreamReader(this.file);//Позволить считывать информацию при выполнении
        max_time = TimeSpan.Parse(sr.ReadLine());//Подсчет времени
        do
        {

            string tmp = sr.ReadLine();
            if (tmp != "")
            {
                result += tmp + "\r\n";

            }
            else
            {
                if (is_path == true)//Если есть путь
                {
                    in_value.Add(result);//Ввести переменную
                }
                else//Если нет
                {
                    out_value.Add(result);//Ввести в результат выходящее значение
                }
                is_path = !is_path;
                result = "";
            }
        } while (!sr.EndOfStream);//Пока не наступил конец проверки
        out_value.Add(result);//Результат добоавить в подготовленную переменную
    }

    /// <summary>
    /// максимальное время для выполнения задания
    /// </summary>
    public TimeSpan MaxTime
    {
        get { return this.max_time; }
    }

    /// <summary>
    /// Список путей к файлам с тестами для даного задания
    /// </summary>
    private List<string> InValue//Забирает входящее значение
    {
        get { return this.in_value; }
    }

    /// <summary>
    /// Список результатов тестов указаных в PathFiles
    /// </summary>
    private List<string> OutValue
    {
        get { return this.out_value; }
    }

    public string Start(string file)
    { Program MakeChoise = new Program();
        int choise = Program.MakeChoise(Program.comp);//Для выбора компилятора
        string tmp = "";
        for (int i = 0; i < this.InValue.Count; i++)//Выполнять по колличеству тестов
        {
            /* switch(choise)
             {
                 case 1: ProcessStartInfo csc_exe = new ProcessStartInfo(file.Replace(".csc", ".exe"));
                     break;
                 case 2: ProcessStartInfo pas_exe = new ProcessStartInfo(file.Replace(".pas", ".exe"));
                     break;
                 default: throw new NotFiniteNumberException("Ошибка при открытии файла!");
                     break;
             }*/
            //ProcessStartInfo exe = new ProcessStartInfo(file.Replace(".pas", ".exe"));
            //ProcessStartInfo exe = ChooseType(int choise);
            ProcessStartInfo exe = ChooseType(choise, file);//Присваиват переменной представляющей компилятор расшерение компилятора и программы
            exe.UseShellExecute = false;//Нужно ли использовать оболочку Windows для проверки программы
            exe.RedirectStandardOutput = true;//Сохранять результаты?
            exe.RedirectStandardInput = true;//Считывать входные значения?
            Process result = Process.Start(exe);//Объявление переменной где будет сохранятся результат выполнения компиляции
            result.StandardInput.Write(this.InValue[i]);//Вводит входные данные

            if (result.StandardOutput.ReadToEnd() == this.OutValue[i])//Если выходные данные равны данным из теста
            {
                tmp += "Тест " + (i + 1) + " пройдений\n";
            }
            else
            {
                tmp += "Тест " + (i + 1) + " не пройдений!\n";
            }
        }
        return tmp;
    }
    public static ProcessStartInfo ChooseType(int choise, string file)//Моя часть:Выбор расширения программы и компилятора
    {
        ProcessStartInfo exe;
        switch (choise)
        {
            case 1:
                exe = new ProcessStartInfo(file.Replace(".cs", ".exe"));
                break;
            case 2:
                exe = new ProcessStartInfo(file.Replace(".pas", ".exe"));
                break;
            case 3:
                exe = new ProcessStartInfo(file.Replace(".pas", ".exe"));
                break;
            default: throw new NotFiniteNumberException("Ошибка при открытии файла!Возможно неверный формат!");

        }
        return exe;
    }
}

}

