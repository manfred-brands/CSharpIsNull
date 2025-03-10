// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CSharpIsNullAnalyzer;
using Xunit;
using VerifyCS = CSharpCodeFixVerifier<CSharpIsNullAnalyzer.CSIsNull002, CSharpIsNullAnalyzer.CSIsNull002Fixer>;

public class CSIsNull002Tests
{
    [Fact]
    public async Task NotEqualsNullInExpressionBody_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    bool Method(object o) => o [|!= null|];
}";

        string fixedSource1 = @"
class Test
{
    bool Method(object o) => o is object;
}";

        string fixedSource2 = @"
class Test
{
    bool Method(object o) => o is not null;
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource1, CSIsNull002Fixer.IsObjectEquivalenceKey);
        await VerifyCS.VerifyCodeFixAsync(source, fixedSource2, CSIsNull002Fixer.IsNotNullEquivalenceKey);
    }

    [Fact]
    public async Task NullNotEqualsInExpressionBody_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    bool Method(object o) => [|null !=|] o;
}";

        string fixedSource1 = @"
class Test
{
    bool Method(object o) => o is object;
}";

        string fixedSource2 = @"
class Test
{
    bool Method(object o) => o is not null;
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource1, CSIsNull002Fixer.IsObjectEquivalenceKey);
        await VerifyCS.VerifyCodeFixAsync(source, fixedSource2, CSIsNull002Fixer.IsNotNullEquivalenceKey);
    }

    [Fact]
    public async Task NotEqualsNullInIfExpression_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    void Method(object o)
    {
        if (o [|!= null|])
        {
        }
    }
}";

        string fixedSource1 = @"
class Test
{
    void Method(object o)
    {
        if (o is object)
        {
        }
    }
}";

        string fixedSource2 = @"
class Test
{
    void Method(object o)
    {
        if (o is not null)
        {
        }
    }
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource1, CSIsNull002Fixer.IsObjectEquivalenceKey);
        await VerifyCS.VerifyCodeFixAsync(source, fixedSource2, CSIsNull002Fixer.IsNotNullEquivalenceKey);
    }

    [Fact]
    public async Task NullNotEqualsInIfExpression_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    void Method(object o)
    {
        if ([|null !=|] o)
        {
        }
    }
}";

        string fixedSource1 = @"
class Test
{
    void Method(object o)
    {
        if (o is object)
        {
        }
    }
}";

        string fixedSource2 = @"
class Test
{
    void Method(object o)
    {
        if (o is not null)
        {
        }
    }
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource1, CSIsNull002Fixer.IsObjectEquivalenceKey);
        await VerifyCS.VerifyCodeFixAsync(source, fixedSource2, CSIsNull002Fixer.IsNotNullEquivalenceKey);
    }

    [Fact]
    public async Task NullNotEqualsInArgument_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    void Method(object o)
    {
        Other([|null !=|] o);
    }

    void Other(bool condition) { }
}";

        string fixedSource1 = @"
class Test
{
    void Method(object o)
    {
        Other(o is object);
    }

    void Other(bool condition) { }
}";

        string fixedSource2 = @"
class Test
{
    void Method(object o)
    {
        Other(o is not null);
    }

    void Other(bool condition) { }
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource1, CSIsNull002Fixer.IsObjectEquivalenceKey);
        await VerifyCS.VerifyCodeFixAsync(source, fixedSource2, CSIsNull002Fixer.IsNotNullEquivalenceKey);
    }

    [Fact]
    public async Task NotEqualsNullInExpressionTree_TargetTyped_OffersOneCodeFix()
    {
        string source = @"
using System;
using System.Linq.Expressions;

class Test
{
    void Method()
    {
        Expression<Func<string, bool>> e = s => s [|!= null|];
    }
}";

        string fixedSource = @"
using System;
using System.Linq.Expressions;

class Test
{
    void Method()
    {
        Expression<Func<string, bool>> e = s => s is object;
    }
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource, CSIsNull002Fixer.IsObjectEquivalenceKey);
        await VerifyCS.VerifyCodeFixAsync(source, source, CSIsNull002Fixer.IsNotNullEquivalenceKey); // assert that this fix is not offered.
    }

    [Fact]
    public async Task NotEqualsNullInExpressionTree_OffersOneCodeFix()
    {
        string source = @"
using System;
using System.Linq.Expressions;

class Test
{
    void Method()
    {
        _ = (Expression<Func<string, bool>>)(s => s [|!= null|]);
    }
}";

        string fixedSource = @"
using System;
using System.Linq.Expressions;

class Test
{
    void Method()
    {
        _ = (Expression<Func<string, bool>>)(s => s is object);
    }
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource, CSIsNull002Fixer.IsObjectEquivalenceKey);
        await VerifyCS.VerifyCodeFixAsync(source, source, CSIsNull002Fixer.IsNotNullEquivalenceKey); // assert that this fix is not offered.
    }
}
