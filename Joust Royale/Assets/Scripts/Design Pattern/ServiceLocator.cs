using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : Singleton<ServiceLocator>
{
    private IDictionary<Type, MonoBehaviour> serviceReferences;
    protected void Awake()
    {
        SingletonBuilder(this);
        serviceReferences = new Dictionary<Type, MonoBehaviour>();
        DontDestroyOnLoad(gameObject);
    }

    public T GetService<T>() where T : MonoBehaviour, new()
    {
        UnityEngine.Assertions.Assert.IsNotNull(serviceReferences, "Someone has requested a service prior to the locator's initialization.");

        Type serviceType = typeof(T);
        if (!serviceReferences.ContainsKey(serviceType))
        {
            T service = FindObjectOfType<T>();
            if (service != null)
            {
                serviceReferences.Add(serviceType, service);
            }
            else
            {
                Debug.LogWarning("Could not find service: " + serviceType);
                return null;
            }
        }

        UnityEngine.Assertions.Assert.IsTrue(serviceReferences.ContainsKey(serviceType), "Could not find service: " + serviceType);
        var serviceInstance = (T)serviceReferences[serviceType];
        UnityEngine.Assertions.Assert.IsNotNull(serviceInstance, serviceType.ToString() + " could not be found.");
        return serviceInstance;
    }

    public void RegisterService<T>(T service) where T : MonoBehaviour
    {
        Type serviceType = typeof(T);
        if (!serviceReferences.ContainsKey(serviceType))
        {
            serviceReferences.Add(serviceType, service);
        }
        else
        {
            serviceReferences[serviceType] = service;
            Debug.LogWarning("Service of type " + serviceType + " is already registered.");
        }
    }

    public void DeregisterService<T>() where T : MonoBehaviour
    {
        Type serviceType = typeof(T);
        if (serviceReferences.ContainsKey(serviceType))
        {
            serviceReferences.Remove(serviceType);
        }
        else
        {
            Debug.LogWarning("Service of type " + serviceType + " is not registered.");
        }
    }
}
