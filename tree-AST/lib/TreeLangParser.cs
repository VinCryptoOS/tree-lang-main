namespace tree_lang;

public partial class TreeLangParser
{
    public int IndentationTabValue = 4;

    public AST ParseFile(FileInfo fi)
    {
        if (!fi.Exists)
            throw new FileNotFoundException("TreeLangParser error: file for parsing not found", fi.FullName);

        var file  = new AST.File(fi);
        var lines = File.ReadAllLines(fi.FullName, System.Text.Encoding.UTF8);

        return ParseFile(new List<string>(lines), file, 0);
    }

    public AST ParseFile(List<string> strings, AST.File file, int indentation = 0)
    {
        // Пустой файл
        if (strings.Count == 0)
            return new AST();


    
        return new AST();
    }
/*
    public void ParseFile_getNonEmptyString(int ref lineNumber, List<string> strings, int indentation = 0)
    {

    }*/

    public (string trimmedLine, int minIndentation, int computedIndentation, int trimmedSymbolsCount)
    TrimPaddings
    (
        string line,
        int indentation, int maxAcceptableIndentation
    )
    {
        int minIndentation      = 0;
        int computedIndentation = 0;

        int i = 0;
        for (; i < line.Length; i++)
        {
            var @char = line[i];

            // Если символ до пробела (служебные или пробел), то мы их считаем за пробельные
            if (@char <= 32)
            {
                if (@char == '\t')
                {
                    computedIndentation += IndentationTabValue;
                    if (minIndentation < indentation)
                        minIndentation += IndentationTabValue;
                }
                else
                {
                    computedIndentation++;
                    if (minIndentation < indentation)
                        minIndentation++;
                }

                continue;
            }

            // Отбросили все пробельные символы и встретили первый непробельный символ
            // Тогда пора заканчивать цикл
            break;
        }

        var trimmedLine = line.Substring(i);

        return (trimmedLine, minIndentation, computedIndentation, i);
    }
}
