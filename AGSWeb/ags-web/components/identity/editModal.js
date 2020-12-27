import { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Label } from 'reactstrap';
import React, { useEffect } from 'react';
import { resposne_success } from '../../config/identity.js'


export default function EditModal({ toggle, isOpen, title, onSaveClick, ConcreteEditModal, concreteEditModalProps, inputData, defaultInputData }) {
    const [error, setError] = useState("");
    const [editData, setEditData] = useState(inputData == null? JSON.parse(JSON.stringify(defaultInputData)) : JSON.parse(JSON.stringify(inputData)));

    useEffect(() => {
        setEditData(inputData == null? JSON.parse(JSON.stringify(defaultInputData)) : JSON.parse(JSON.stringify(inputData)))
    }, [inputData])

    const onEditSaveClick = async (e) => {
        const result = await onSaveClick(editData);
        
        if (result.code != resposne_success){
            setError(result.code);
        }else {
            setError("");
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
                <Button color="primary" onClick={onEditSaveClick} type="button">Save</Button>
                <Button color="secondary" onClick={toggle}>Cancel</Button>
            </ModalFooter>
        </Modal>
    )
}