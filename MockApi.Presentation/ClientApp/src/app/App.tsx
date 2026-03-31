import { useState } from 'react';
import { Sun, Moon, Send, Copy, Check, Clock, ArrowRight, Terminal, FileJson, Sparkles } from 'lucide-react';

interface RequestHistory {
  method: 'schema' | 'ai';
  timestamp: string;
  status: 'success' | 'error';
}

const schemaPlaceholder = `{
  "schema": {
    "fullName": "String",
    "email": "String",
    "age": "Integer",
    "isActive": "Boolean",
    "createdAt": "DateTime",
    "profile": {
      "city": "String",
      "birthday": "Date"
    }
  }
}`;

const aiPlaceholder = 'Сгенерируй JSON профиля клиента банка с ФИО, номером счета, балансом, датой открытия и вложенным объектом контактных данных.';

const fieldTypes = ['String', 'Integer', 'Boolean', 'Date', 'DateTime'];
const apiBaseUrl = (import.meta.env.VITE_API_BASE_URL ?? '').trim().replace(/\/$/, '');

export default function App() {
  const [theme, setTheme] = useState<'light' | 'dark'>('light');
  const [selectedMethod, setSelectedMethod] = useState<'schema' | 'ai'>('schema');
  const [inputValue, setInputValue] = useState(schemaPlaceholder);
  const [outputJson, setOutputJson] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [copied, setCopied] = useState(false);
  const [history, setHistory] = useState<RequestHistory[]>([]);

  const toggleTheme = () => {
    setTheme(theme === 'light' ? 'dark' : 'light');
  };

  const setMode = (mode: 'schema' | 'ai') => {
    setSelectedMethod(mode);
    setInputValue(mode === 'schema' ? schemaPlaceholder : aiPlaceholder);
    setOutputJson('');
  };

  const addHistory = (method: 'schema' | 'ai', status: 'success' | 'error') => {
    setHistory((prev) => [
      {
        method,
        timestamp: new Date().toLocaleTimeString('ru-RU'),
        status,
      },
      ...prev.slice(0, 4),
    ]);
  };

  const handleSubmit = async () => {
    if (!inputValue.trim()) {
      return;
    }

    setIsLoading(true);

    try {
      let response: Response;
      let formattedResponse: string;

      if (selectedMethod === 'schema') {
        const parsedBody = JSON.parse(inputValue);
        response = await fetch(`${apiBaseUrl}/api/Mock`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(parsedBody),
        });

        const data = await response.json();
        formattedResponse = JSON.stringify(data, null, 2);
      } else {
        response = await fetch(`${apiBaseUrl}/api/Mock/ai?description=${encodeURIComponent(inputValue)}`, {
          method: 'POST',
        });

        const text = await response.text();

        try {
          formattedResponse = JSON.stringify(JSON.parse(text), null, 2);
        } catch {
          formattedResponse = text;
        }
      }

      if (!response.ok) {
        throw new Error(formattedResponse || `HTTP ${response.status}`);
      }

      setOutputJson(formattedResponse);
      addHistory(selectedMethod, 'success');
    } catch (error) {
      const message =
        error instanceof SyntaxError
          ? 'Некорректный JSON во входных данных.'
          : error instanceof TypeError
            ? `Connection failure. Проверьте, что бэкенд запущен и доступен по ${apiBaseUrl || 'текущему origin или Vite proxy на http://localhost:5255/api'}.`
          : error instanceof Error
            ? error.message
            : 'Не удалось выполнить запрос.';

      setOutputJson(
        JSON.stringify(
          {
            error: message,
          },
          null,
          2,
        ),
      );
      addHistory(selectedMethod, 'error');
    } finally {
      setIsLoading(false);
    }
  };

  const handleCopy = async () => {
    await navigator.clipboard.writeText(outputJson);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  const endpoint = selectedMethod === 'schema' ? 'POST /Mock' : 'POST /Mock/ai?description=...';
  const methodLabel = selectedMethod === 'schema' ? 'По схеме' : 'Через AI';
  const inputLabel = selectedMethod === 'schema' ? 'Тело запроса' : 'Описание для генерации';
  const inputHint =
    selectedMethod === 'schema'
      ? 'Отправьте объект вида { "schema": { ... } }'
      : 'Опишите желаемую структуру или готовые данные обычным текстом';

  return (
    <div className={theme === 'dark' ? 'dark' : ''}>
      <div className="min-h-screen bg-background text-foreground transition-colors duration-300">
        <div className="fixed inset-0 pointer-events-none overflow-hidden">
          <div className="absolute -top-40 -right-40 w-80 h-80 bg-primary/10 rounded-full blur-3xl" />
          <div className="absolute top-1/2 -left-40 w-96 h-96 bg-accent/10 rounded-full blur-3xl" />
          <div className="absolute -bottom-40 right-1/4 w-80 h-80 bg-secondary/20 rounded-full blur-3xl" />
        </div>

        <header className="relative border-b border-border backdrop-blur-sm bg-card/50">
          <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6">
            <div className="flex items-center justify-between gap-4">
              <div className="flex items-center gap-4">
                <div className="p-3 bg-gradient-to-br from-primary to-accent rounded-xl shadow-lg shadow-primary/20">
                  <Terminal className="w-6 h-6 text-white" />
                </div>
                <div>
                  <h1 className="text-2xl font-medium bg-gradient-to-r from-primary to-accent bg-clip-text text-transparent">
                    Mock API Console
                  </h1>
                  <p className="text-sm text-muted-foreground mt-0.5">Фронтенд для рабочего бэкенда генерации JSON</p>
                </div>
              </div>

              <button
                onClick={toggleTheme}
                className="p-3 rounded-xl hover:bg-muted transition-all hover:scale-105"
                aria-label="Toggle theme"
              >
                {theme === 'light' ? <Moon className="w-5 h-5" /> : <Sun className="w-5 h-5" />}
              </button>
            </div>
          </div>
        </header>

        <main className="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
            <div className="lg:col-span-1 space-y-6">
              <div className="bg-card border border-border rounded-2xl p-6 shadow-lg">
                <div className="flex items-center gap-2 mb-4">
                  <FileJson className="w-5 h-5 text-primary" />
                  <h3 className="font-medium">Режим запроса</h3>
                </div>

                <div className="space-y-3">
                  <button
                    onClick={() => setMode('schema')}
                    className={`w-full py-3 px-4 rounded-xl transition-all text-left flex items-center justify-between group ${
                      selectedMethod === 'schema'
                        ? 'bg-gradient-to-r from-primary to-accent text-white shadow-lg shadow-primary/30'
                        : 'bg-muted/50 hover:bg-muted border border-border'
                    }`}
                  >
                    <span className="font-medium">Генерация по схеме</span>
                    <ArrowRight
                      className={`w-4 h-4 transition-transform ${
                        selectedMethod === 'schema' ? 'translate-x-1' : 'group-hover:translate-x-1'
                      }`}
                    />
                  </button>

                  <button
                    onClick={() => setMode('ai')}
                    className={`w-full py-3 px-4 rounded-xl transition-all text-left flex items-center justify-between group ${
                      selectedMethod === 'ai'
                        ? 'bg-gradient-to-r from-primary to-accent text-white shadow-lg shadow-primary/30'
                        : 'bg-muted/50 hover:bg-muted border border-border'
                    }`}
                  >
                    <span className="font-medium">AI генерация</span>
                    <ArrowRight
                      className={`w-4 h-4 transition-transform ${
                        selectedMethod === 'ai' ? 'translate-x-1' : 'group-hover:translate-x-1'
                      }`}
                    />
                  </button>
                </div>

                <div className="mt-6 space-y-4">
                  <div className="p-4 bg-muted/30 rounded-lg border border-border/50">
                    <p className="text-xs text-muted-foreground mb-1">Endpoint</p>
                    <code className="text-xs font-mono text-primary">{endpoint}</code>
                  </div>

                  {selectedMethod === 'schema' && (
                    <div className="p-4 bg-muted/30 rounded-lg border border-border/50">
                      <p className="text-xs text-muted-foreground mb-2">Поддерживаемые типы</p>
                      <div className="flex flex-wrap gap-2">
                        {fieldTypes.map((type) => (
                          <code key={type} className="text-xs px-2 py-1 rounded-md bg-background border border-border">
                            {type}
                          </code>
                        ))}
                      </div>
                    </div>
                  )}
                </div>
              </div>

              {history.length > 0 && (
                <div className="bg-card border border-border rounded-2xl p-6 shadow-lg">
                  <div className="flex items-center gap-2 mb-4">
                    <Clock className="w-5 h-5 text-accent" />
                    <h3 className="font-medium">История</h3>
                  </div>
                  <div className="space-y-2">
                    {history.map((item, index) => (
                      <div
                        key={`${item.timestamp}-${index}`}
                        className="flex items-center justify-between p-3 bg-muted/30 rounded-lg border border-border/50 text-sm"
                      >
                        <div className="flex items-center gap-2">
                          <div className={`w-2 h-2 rounded-full ${item.status === 'success' ? 'bg-success' : 'bg-destructive'}`} />
                          <span className="text-xs">{item.method === 'schema' ? 'Схема' : 'AI'}</span>
                        </div>
                        <span className="text-xs text-muted-foreground">{item.timestamp}</span>
                      </div>
                    ))}
                  </div>
                </div>
              )}
            </div>

            <div className="lg:col-span-3 space-y-6">
              <div className="bg-card border border-border rounded-2xl p-5 shadow-lg">
                <div className="flex flex-col gap-2 sm:flex-row sm:items-center sm:justify-between">
                  <div>
                    <p className="text-sm font-medium">{methodLabel}</p>
                    <p className="text-sm text-muted-foreground">{inputHint}</p>
                  </div>
                  <div className="inline-flex items-center gap-2 text-xs text-muted-foreground px-3 py-2 bg-muted/40 rounded-full">
                    <Sparkles className="w-4 h-4" />
                    <span>{selectedMethod === 'schema' ? 'Бэкенд дополнит пустые типы при необходимости' : 'Ответ приходит от AI-эндпоинта как JSON-строка'}</span>
                  </div>
                </div>
              </div>

              <div className="grid grid-cols-1 xl:grid-cols-2 gap-6">
                <div className="space-y-4">
                  <div className="flex items-center justify-between gap-4">
                    <label className="flex items-center gap-2 font-medium">
                      <div className="w-8 h-8 rounded-lg bg-primary/10 flex items-center justify-center">
                        <ArrowRight className="w-4 h-4 text-primary rotate-180" />
                      </div>
                      {inputLabel}
                    </label>
                    <span className="text-xs text-muted-foreground px-3 py-1 bg-muted/50 rounded-full">Input</span>
                  </div>

                  <div className="relative">
                    <div className="absolute inset-0 bg-gradient-to-br from-primary/5 to-accent/5 rounded-2xl blur-xl" />
                    <textarea
                      value={inputValue}
                      onChange={(e) => setInputValue(e.target.value)}
                      placeholder={selectedMethod === 'schema' ? schemaPlaceholder : aiPlaceholder}
                      className="relative w-full h-[450px] p-6 bg-card border-2 border-border rounded-2xl font-mono text-sm resize-none focus:outline-none focus:border-primary/50 focus:ring-4 focus:ring-primary/10 transition-all shadow-lg"
                    />
                  </div>

                  <button
                    onClick={handleSubmit}
                    disabled={isLoading || !inputValue.trim()}
                    className="w-full bg-gradient-to-r from-accent to-primary text-white py-4 px-6 rounded-xl hover:shadow-xl disabled:opacity-50 disabled:cursor-not-allowed transition-all flex items-center justify-center gap-3 shadow-lg shadow-accent/30 hover:scale-[1.02] active:scale-[0.98] group relative overflow-hidden"
                  >
                    <div className="absolute inset-0 bg-gradient-to-r from-primary to-accent opacity-0 group-hover:opacity-100 transition-opacity" />
                    {isLoading ? (
                      <>
                        <div className="relative w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin" />
                        <span className="relative">Обработка...</span>
                      </>
                    ) : (
                      <>
                        <Send className="relative w-5 h-5 group-hover:translate-x-1 transition-transform" />
                        <span className="relative font-medium">Отправить запрос</span>
                      </>
                    )}
                  </button>
                </div>

                <div className="space-y-4">
                  <div className="flex items-center justify-between">
                    <label className="flex items-center gap-2 font-medium">
                      <div className="w-8 h-8 rounded-lg bg-success/10 flex items-center justify-center">
                        <Check className="w-4 h-4 text-success" />
                      </div>
                      Результат
                    </label>
                    <div className="flex items-center gap-2">
                      <span className="text-xs text-muted-foreground px-3 py-1 bg-muted/50 rounded-full">Output</span>
                      {outputJson && (
                        <button
                          onClick={handleCopy}
                          className="p-2 rounded-lg hover:bg-muted transition-all hover:scale-105 flex items-center gap-2 text-sm group"
                        >
                          {copied ? (
                            <>
                              <Check className="w-4 h-4 text-success" />
                              <span className="text-xs text-success">Скопировано</span>
                            </>
                          ) : (
                            <>
                              <Copy className="w-4 h-4 group-hover:text-primary transition-colors" />
                              <span className="text-xs hidden sm:inline">Копировать</span>
                            </>
                          )}
                        </button>
                      )}
                    </div>
                  </div>

                  <div className="relative">
                    <div className="absolute inset-0 bg-gradient-to-br from-success/5 to-primary/5 rounded-2xl blur-xl" />
                    <div className="relative">
                      <pre className="w-full h-[450px] p-6 bg-card border-2 border-border rounded-2xl font-mono text-sm overflow-auto shadow-lg whitespace-pre-wrap break-words">
                        {outputJson || (
                          <span className="text-muted-foreground flex flex-col items-center justify-center h-full text-center">
                            <FileJson className="w-12 h-12 mb-3 opacity-20" />
                            <span>Результат появится здесь после выполнения запроса</span>
                          </span>
                        )}
                      </pre>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </main>
      </div>
    </div>
  );
}
