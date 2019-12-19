# CitaviAddOnEx

This repository contains a class library that extends the addon programming model of Swiss Academic Citavi to support all dialogs inherited from the `FormBase` class.

## Usage

```csharp
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace [NAMESPACE]
{
    public class [CLASSNAME] : CitaviAddOnEx<T>
    {
        // Call through System.Windows.Forms.Application.Idle event and can used to check if as example button states changed
        public override void OnApplicationIdle(T form)
        {
            base.OnApplicationIdle(form);
        }
       
        // Called for every form of T when its load
        public override void OnHostingFormLoaded(T form)
        {
            base.OnHostingFormLoaded(form);
        }

        // Call when user click on something in the form
        public override void OnBeforePerformingCommand(T form, BeforePerformingCommandEventArgs e)
        {
            base.OnBeforePerformingCommand(form, e);
        }

        // Call when application language is changed
        public override void OnLocalizing(T form)
        {
            base.OnLocalizing(form);
        }
    }
}
```

To program an addon you derive from the `CitaviAddOnEx<T>` class and implement the corresponding methods if required. `T` must be a Form derived from the Citavi model provided base class `FormBase`.

**Example**
- MainForm
- KnowledgeItemFileForm
- MacroEditor

If a form is specified that provides inherent add-on support, the class will use it.

## Disclaimer

>There are no support claims by the company **Swiss Academic Software GmbH**, the provider of **Citavi** or other liability claims for problems or data loss. Any use is at your own risk. All rights to the name **Citavi** and any logos used are owned by **Swiss Academic Software GmbH**.

## License

This project is licensed under the [MIT](LICENSE) License
