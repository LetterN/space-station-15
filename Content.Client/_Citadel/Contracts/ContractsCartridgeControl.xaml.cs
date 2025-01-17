﻿using System.Linq;
using Content.Shared._Citadel.Contracts;
using Content.Shared._Citadel.Contracts.BUI;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client._Citadel.Contracts;

[GenerateTypedNameReferences]
public sealed partial class ContractsCartridgeControl : TabContainer
{

    public event Action<Guid>? OnStartContract;
    public event Action<Guid>? OnCancelContract;
    public event Action<Guid>? OnJoinContract;
    public event Action<Guid>? OnLeaveContract;
    public event Action<Guid>? OnInitiateContract;
    public event Action<Guid>? OnHailContract;

    public ContractsCartridgeControl()
    {
        RobustXamlLoader.Load(this);
        SetTabTitle(0, "Contracts");
        SetTabTitle(1, "Bank Account");

        MainContractList.OnStartContract += guid => OnStartContract?.Invoke(guid);
        MainContractList.OnCancelContract += guid => OnCancelContract?.Invoke(guid);
        MainContractList.OnJoinContract += guid => OnJoinContract?.Invoke(guid);
        MainContractList.OnLeaveContract += guid => OnLeaveContract?.Invoke(guid);
        MainContractList.OnStartContract += guid => OnStartContract?.Invoke(guid);
        MainContractList.OnInitiateContract += guid => OnInitiateContract?.Invoke(guid);
        MainContractList.OnHailContract += guid => OnHailContract?.Invoke(guid);
    }

    public void Update(ContractCartridgeUiState uiState)
    {
        MainContractList.Update(uiState.Contracts.Contracts
            .OrderBy(x => x.Value.Status)
            .ThenBy(x => x.Value.UserStatus)
            .Where(x => x.Value.Status is not ContractStatus.Breached and not ContractStatus.Finalized and not ContractStatus.Cancelled
                        || x.Value.UserStatus is ContractUiState.ContractUserStatus.Subcontractor or ContractUiState.ContractUserStatus.Owner)
        );
        BankAmount.Text = uiState.Contracts.BankAccount.ToString();
    }
}
