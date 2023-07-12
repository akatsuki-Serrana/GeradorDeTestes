﻿using FluentResults;

namespace GeradorDeTestes.WinApp.Compartilhado
{

    public delegate Result GravarRegistroDelegate<T>(T registro) where T : EntidadeBase<T>;

}