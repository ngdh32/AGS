import React, { Component, ComponentType } from "react";
import EditModalResult from "../../common/editModalResult";

export interface EditModalProps
{
    toggle: (event: React.KeyboardEvent<any> | React.MouseEvent<any, MouseEvent>) => void
    isOpen: boolean
    title: string
    onSaveClick: (editData: any) => Promise<EditModalResult>
    ConcreteEditModal: ComponentType<{editData: any, setEditData: React.Dispatch<any>, concreteEditModalProps: any}>
    concreteEditModalProps: any
    inputData: any
    defaultInputData: any
}