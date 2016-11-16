using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
	public enum LoginResultType
	{
		UnknownError,
		UnknownServerName
	}

	public enum ExceptionPolicyName
	{
		WarningPolicy,
		ErrorPolicy,
		CriticaPolicy
	}
}
