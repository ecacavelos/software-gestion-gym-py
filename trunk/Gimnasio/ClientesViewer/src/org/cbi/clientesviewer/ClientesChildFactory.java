/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package org.cbi.clientesviewer;

import java.beans.IntrospectionException;
import java.util.List;
import lib.Clientes;
import org.openide.nodes.BeanNode;
import org.openide.nodes.ChildFactory;
import org.openide.nodes.Node;
import org.openide.util.Exceptions;

public class ClientesChildFactory extends ChildFactory<Clientes> {

    private List<Clientes> resultList;

    public ClientesChildFactory(List<Clientes> resultList) {
        this.resultList = resultList;
    }

    @Override
    protected boolean createKeys(List<Clientes> list) {
        for (Clientes Clientes : resultList) {
            list.add(Clientes);
        }
        return true;
    }

    /*Aca deberia crear cada nodo con el nombre que tiene que ser.*/
    @Override
    protected Node createNodeForKey(Clientes c) {
        try {
            BeanNode nodeClienteLeaf = new BeanNode(c); 
            nodeClienteLeaf.setDisplayName(c.getNombre());
            return nodeClienteLeaf;
        } catch (IntrospectionException ex) {
            Exceptions.printStackTrace(ex);
            return null;
        }
    }

}
