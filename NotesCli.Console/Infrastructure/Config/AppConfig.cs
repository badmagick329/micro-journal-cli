using NotesCli.Console.Core;

namespace NotesCli.Console.Infrastructure.Config;

record class AppConfig(string NotesRoot, List<Category> Categories);
