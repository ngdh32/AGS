import { FunctionClaimItemType } from "../../identity/functionClaimItemType";

export interface FunctionClaimBadgeProps
{
    functionClaim: FunctionClaimItemType
    onFunctionClaimBadgeRemoveClick: (functionClaimId: string) => void
}