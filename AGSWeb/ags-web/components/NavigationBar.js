import {
    Collapse,
    Navbar,
    NavbarToggler,
    NavbarBrand,
    Nav,
    NavItem,
    NavLink,
    UncontrolledDropdown,
    DropdownToggle,
    DropdownMenu,
    DropdownItem,
    NavbarText, 
    Button,
    ButtonGroup
} from 'reactstrap'
import React, { useContext, useState } from 'react';
import menus from '../config/Menu.js'
import { AGSContext } from '../helpers/common/agsContext.js'
import { locale_Options } from '../config/localization.js'
import { SetLocaleCookieInClient, GetLocalizedString } from '../helpers/common/localizationHelper.js'

export default function NavigationBar() {
    const agsContext = useContext(AGSContext);
    let menuOptions = JSON.parse(JSON.stringify(menus));
    menuOptions = GetValidMenuOptions(menuOptions, agsContext.functionClaims);
    
    const [isOpen, setIsOpen] = useState(false);
    const toggle = () => setIsOpen(!isOpen);
 
    return (
        <Navbar color="light" light expand="md">
            <NavbarBrand><h1>AGS</h1></NavbarBrand>
            <NavbarToggler onClick={toggle} />
            <Collapse isOpen={isOpen} navbar>
                <Nav className="mr-auto" navbar>
                    {
                        menuOptions.map(x => {
                            return (
                                <NavigationItem menuOption={x} level={0} />
                            )
                        })
                    }
                </Nav>
                <NavbarText>
                    <LocaleButtonGroups />
                    <a href="/auth/Logout">Logout</a>
                </NavbarText>
            </Collapse>
        </Navbar>
    )
}

function LocaleButtonGroups(){
    const agsContext = useContext(AGSContext);
    
    function OnClickLocaleButton(locale){
        SetLocaleCookieInClient(locale)
        location.reload();
    }
    
    return (
        <ButtonGroup>
            {
                locale_Options.map(x => {
                    return (
                        <Button color="primary" active={agsContext.locale == x.value} onClick={() => { OnClickLocaleButton(x.value) }} >{x.label}</Button>
                    )
                })
            }
        </ButtonGroup>
    )
}

function NavigationItem({ menuOption, level }) {
    if (menuOption.ChildrenMenus == undefined || menuOption.ChildrenMenus.length == 0) {
        if (level == 0) {
            return (
                <NavItem>
                    <NavLink href={menuOption.Url}>{GetLocalizedString(menuOption.LabelKey)}</NavLink>
                </NavItem>
            )
        } else {
            return (
                <DropdownItem><a href={menuOption.Url}>{GetLocalizedString(menuOption.LabelKey)}</a></DropdownItem>
            )
        }
    }
 
    if (menuOption.ChildrenMenus != undefined && menuOption.ChildrenMenus.length > 0) {
        return (
            <UncontrolledDropdown inNavbar direction={level == 0 ? "down" : "right"}>
                <DropdownToggle nav caret>
                    {GetLocalizedString(menuOption.LabelKey)}
                </DropdownToggle>
                <DropdownMenu>
                    {
                        menuOption.ChildrenMenus.map(x => {
                            return (
                                <NavigationItem level={level + 1} menuOption={x} />
                            )
                        })
                    }
                </DropdownMenu>
            </UncontrolledDropdown>
        )
    }
}
 
function GetValidMenuOptions(menuOptions, functionClaims){
    const validMenuOptions = [];
    menuOptions.forEach(x => {
        if (x.FunctionClaim == "" || functionClaims.some(y => y == x.FunctionClaim)){
            validMenuOptions.push(x);
        }
    });

    validMenuOptions.forEach(x => {
        x.ChildrenMenus = GetValidMenuOptions(x.ChildrenMenus, functionClaims);
    })

    return validMenuOptions;
}

