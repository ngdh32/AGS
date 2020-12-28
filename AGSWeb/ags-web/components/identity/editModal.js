import { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Label } from 'reactstrap';
import React, { useEffect } from 'react';
import { resposne_success } from '../../config/identity.js'
import { GetLocalizedString } from '../../helpers/common/localizationHelper.js'


export default function EditModal({ toggle, isOpen, title, onSaveClick, ConcreteEditModal, concreteEditModalProps, inputData, defaultInputData }) {
    const [error, setError] = useState("");
    const [editData, setEditData] = useState(inputData == null? JSON.parse(JSON.stringify(defaultInputData)) : JSON.parse(JSON.stringify(inputData)));

    useEffect(() => {
        setEditData(inputData == null? JSON.parse(JSON.stringify(defaultInputData)) : JSON.parse(JSON.stringify(inputData)))
    }, [inputData])

    const onEditSaveClick = async (e) => {
        setError("");

        const result = await onSaveClick(editData);
        console.log(result)
        if (result.data.code != resposne_success){
            setError(result.data.code);
            return;
        }

        location.reload();
    }

    return (
        <Modal isOpen={isOpen} toggle={toggle} keyboard={false} backdrop={false} title={title}>
            <ModalHeader toggle={toggle}>
                {title}
            </ModalHeader>
            <ConcreteEditModal editData={editData} setEditData={setEditData} {...concreteEditModalProps} />
            <ModalFooter>
                <Label className="text-danger">{error}</Label>
                <Button color="primary" onClick={onEditSaveClick} type="button">{GetLocalizedString("label_common_button_confirm")}</Button>
                <Button color="secondary" onClick={toggle}>{GetLocalizedString("label_common_button_cancel")}</Button>
            </ModalFooter>
        </Modal>
    )
}