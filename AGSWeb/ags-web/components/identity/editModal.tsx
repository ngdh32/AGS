import { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Label } from 'reactstrap';
import React, { useEffect } from 'react';
import { resposne_success } from '../../config/identity'
import { GetLocalizedString } from '../../helpers/common/localizationHelper'
import { EditModalProps } from '../../models/components/identity/editModalProps'


export default function EditModal({ toggle, isOpen, title, onSaveClick, concreteEditModal, concreteEditModalProps, inputData, defaultInputData } : EditModalProps) {
    const [error, setError] = useState("");
    const [editData, setEditData] = useState(inputData == null? JSON.parse(JSON.stringify(defaultInputData)) : JSON.parse(JSON.stringify(inputData)));
    const [isSaving, setIsSaving] = useState(false);

    useEffect(() => {
        setEditData(inputData == null? JSON.parse(JSON.stringify(defaultInputData)) : JSON.parse(JSON.stringify(inputData)))
    }, [inputData])

    const onEditSaveClick = async (e: React.MouseEvent<any, MouseEvent>) : Promise<void> => {
        setError("");
        setIsSaving(true);

        const result = await onSaveClick(editData);
        if (!result.isSuccessful){
            setError(result.errorMessage);
            setIsSaving(false)
            return;
        }

        location.reload();
    }

    const ConcreteEditModal : React.FC = () => <>{concreteEditModal}</>

    return (
        <Modal isOpen={isOpen} toggle={toggle} keyboard={false} backdrop={false} title={title}>
            <ModalHeader toggle={toggle}>
                {title}
            </ModalHeader>
            <ConcreteEditModal editData={editData} setEditData={setEditData} {...concreteEditModalProps} />
            <ModalFooter>
                <Label className="text-danger">{error}</Label>
                <Button disabled={isSaving} color="primary" onClick={onEditSaveClick} type="button">{GetLocalizedString("label_common_button_confirm")}</Button>
                <Button disabled={isSaving} color="secondary" onClick={toggle}>{GetLocalizedString("label_common_button_cancel")}</Button>
            </ModalFooter>
        </Modal>
    )
}