using System.Text;

namespace tree_lang;


public partial class AST
{
    /// <summary>Информация о файле, который послужил источником AST</summary>
    public class File
    {                                                        /// <summary>Файл, который послужил источником AST. Если не существует, то null</summary>
        public readonly FileInfo? file          = null;      /// <summary>Если сработал include, то parentFile содержит информацию о файле, из которого был вызов данного файла</summary>
        public readonly AST.File? parentFile    = null;      /// <summary>Полное имя файла (с полным путём). Если файла не существует, это может быть логическое имя</summary>
        public readonly string    FullName;                  /// <summary>Сокращённое имя файла (имя файла без пути). Если файла не существует, это может быть логическое имя</summary>
        public readonly string    shortName;

        /// <summary>Создаёт описатель источника информации для AST в случае, если источником служил реальный файл</summary>
        /// <param name="file">Реально существующий в файловой системе файл</param>
        /// <param name="parentFile">Файл, в который включается данный файл, если это был include. Иначе - null</param>
        public File(FileInfo file, AST.File? parentFile = null)
        {
            this.file       = file;
            this.FullName   = file.FullName;
            this.shortName  = file.Name;
            this.parentFile = parentFile;
        }

        /// <summary>Создаёт описатель источника информации для AST в случае, если реального существующего файла нет в наличии</summary>
        /// <param name="FullName">Полное логическое имя файла. Допустима пустая стрка</param>
        /// <param name="shortName">Сокращённое логическое имя файла. Если null, будет совпадать с FullName</param>
        /// <param name="parentFile">Файл, в который включается данный файл, если это был include. Иначе - null</param>
        public File(string FullName = "", string? shortName = null, AST.File? parentFile = null)
        {
            this.FullName   = FullName;
            this.shortName  = shortName ?? FullName;
            this.parentFile = parentFile;
        }

        /// <summary>Возвращает сокращённое имя файла. parentPath - это путь к родительской директории, который отсекается от имени файла слева</summary>
        /// <value>Окончание имя файла (без родительской директории)</value>
        public string this[string parentPath]
        {
            get
            {
                var str = FullName;
                if (!str.StartsWith(parentPath))
                    return str;

                return str.Substring(parentPath.Length);
            }
        }

        /// <summary>Возвращает сокращённое имя файла</summary>
        /// <remarks><para>i == -1 - FullName</para><para>0 - shortName</para><para>+1 - дополнительно одна директория к имени файла</para></remarks>
        /// <value>Сокращённое имя файла</value>
        public string this[int i]
        {
            get
            {
                if (file != null)
                {
                    if (i == 0)
                        return file.Name;
                    if (i < 0)
                        return file.FullName;
                    
                    var str = file.Name;
                    var dir = file.Directory;
                    for (var j = 0; j < i && dir != null; j++)
                    {
                        str = Path.Combine(dir.Name, file.Name);
                        dir = dir.Parent;
                    }

                    return str;
                }

                if (i == 0)
                    return shortName;

                return FullName;
            }
        }
    }
}
