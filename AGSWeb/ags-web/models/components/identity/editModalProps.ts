import React from "react";
import EditModalResult from "../../common/editModalResult";

export interface EditModalProps
{
    toggle: (event: React.KeyboardEvent<any> | React.MouseEvent<any, MouseEvent>) => void
    isOpen: boolean
    title: string
    onSaveClick: (editData: any) => Promise<EditModalResult>
    concreteEditModal: React.ReactNode
    concreteEditModalProps: any
    inputData: any
    defaultInputData: any
}