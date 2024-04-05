using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
namespace GameCore.Core
{
    public static class Utilities
    {
        public enum Axis
        {
            x,
            y,
            z
        }
        public static double SystemTimeInMilliseconds { get { return (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalMilliseconds; } }
        public static float WorldWidth { get { return 2f * Camera.main.orthographicSize * Camera.main.aspect; } }
        public static float WorldHeight { get { return 2f * Camera.main.orthographicSize; } }
        public static float XScale { get { return (float)UnityEngine.Screen.width / 1080f; } }
        public static float YScale { get { return (float)UnityEngine.Screen.height / 1920f; } }

        public static void DelayedCall(float delay, UnityAction _action)
        {
            CoroutineStarter.Start(DODelayedCall(delay, _action));
        }
        static IEnumerator DODelayedCall(float delay, UnityAction _action)
        {
            yield return new WaitForSeconds(delay);
            _action();
        }
        public static T GetRandomItemFromArray<T>(this T[] target)
        {
            int _randomInt = Random.Range(0, target.Length);
            return target[_randomInt];
        }
        public static T GetRandomItemFromList<T>(this List<T> target)
        {
            int _randomInt = Random.Range(0, target.Count);
            return target[_randomInt];
        }
        public static T GetRandomItemFromArray<T>(this T[] target, T _exception)
        {
            int _randomInt = Random.Range(0, target.Length);
            while (EqualityComparer<T>.Default.Equals(target[_randomInt], _exception))
            {
                _randomInt = Random.Range(0, target.Length);
            }
            return target[_randomInt];
        }
        public static T GetRandomItemFromList<T>(this List<T> target, T _exception)
        {
            int _randomInt = Random.Range(0, target.Count);
            while (EqualityComparer<T>.Default.Equals(target[_randomInt], _exception))
            {
                _randomInt = Random.Range(0, target.Count);
            }
            return target[_randomInt];
        }
        public static T GetRandomItemFromArray<T>(this T[] target, T[] _exceptions)
        {
            int _randomInt = Random.Range(0, target.Length);
            while (_exceptions.Contains(target[_randomInt]))
            {
                _randomInt = Random.Range(0, target.Length);
            }
            return target[_randomInt];
        }
        public static T GetRandomItemFromList<T>(this List<T> target, T[] _exceptions)
        {
            int _randomInt = Random.Range(0, target.Count);
            while (_exceptions.Contains(target[_randomInt]))
            {
                _randomInt = Random.Range(0, target.Count);
            }
            return target[_randomInt];
        }
        public static Vector3 Vector3Clamp(Vector3 _pos, Vector3 _minPos, Vector3 _maxPos)
        {
            float _xPos = Mathf.Clamp(_pos.x, _minPos.x, _maxPos.x);
            float _yPos = Mathf.Clamp(_pos.y, _minPos.y, _maxPos.y);
            float _zPos = Mathf.Clamp(_pos.z, _minPos.z, _maxPos.z);
            return new Vector3(_xPos, _yPos, _zPos);
        }
        public static Vector3 EulerAngleClamp(Vector3 _pos, Vector3 _minPos, Vector3 _maxPos)
        {
            float _xPos = 0;
            float _yPos = 0;
            float _zPos = 0;
            if (_pos.x > 180)
                _xPos = Mathf.Clamp(_pos.x, 360 + _minPos.x, 361);
            else
                _xPos = Mathf.Clamp(_pos.x, -1, _maxPos.x);

            if (_pos.y > 180)
                _yPos = Mathf.Clamp(_pos.y, 360 + _minPos.y, 361);
            else
                _yPos = Mathf.Clamp(_pos.y, -1, _maxPos.y);

            if (_pos.z > 180)
                _zPos = Mathf.Clamp(_pos.z, 360 + _minPos.z, 361);
            else
                _zPos = Mathf.Clamp(_pos.z, -1, _maxPos.z);
            return new Vector3(_xPos, _yPos, _zPos);
        }
        public static void LerpLocalPosition(this Transform target, Vector3 _targetPos, float _t)
        {
            target.localPosition = Vector3.Lerp(target.transform.localPosition, _targetPos, _t);
        }
        public static void LerpPosition(this Transform target, Vector3 _targetPos, float _t)
        {
            target.position = Vector3.Lerp(target.transform.position, _targetPos, _t);
        }
        public static void LerpLocalRotation(this Transform target, Vector3 _eulerAngles, float _t)
        {
            target.localEulerAngles = Vector3.Lerp(target.localRotation.eulerAngles, _eulerAngles, _t);
        }
        public static void LerpRotation(this Transform target, Vector3 _eulerAngles, float _t)
        {
            target.rotation = Quaternion.Slerp(target.rotation, Quaternion.Euler(_eulerAngles), _t);
        }
        public static void AddIfNotExists<T>(this List<T> _list, T _item)
        {
            if (!_list.Contains(_item))
            {
                _list.Add(_item);
            }
        }

        public static Vector3 RandomVector3(Vector3 _min, Vector3 _max)
        {
            float _x = Random.Range(_min.x, _max.x);
            float _y = Random.Range(_min.y, _max.y);
            float _z = Random.Range(_min.z, _max.z);
            return new Vector3(_x, _y, _z);
        }
        public static float GetPercent(float _count, float _totalCount)
        {
            return (_count * 100) / _totalCount;
        }
        public static Vector3 EditAxis(this Vector3 target, Axis _axis, float _value)
        {

            if (_axis == Axis.x)
            {
                target = new Vector3(_value, target.y, target.z);
            }
            else if (_axis == Axis.y)
            {
                target = new Vector3(target.x, _value, target.z);
            }
            else if (_axis == Axis.z)
            {
                target = new Vector3(target.x, target.y, _value);
            }
            return target;
        }
        public static T GetNext<T>(this List<T> _list, T _item)
        {
            if (_list.Contains(_item))
            {
                int _index = _list.IndexOf(_item) + 1;
                if (_index < _list.Count)
                {
                    return _list[_index];
                }
                else
                {
                    return default(T);
                }
            }
            return default(T);
        }
        public static T GetPrevious<T>(this List<T> _list, T _item)
        {
            if (_list.Contains(_item))
            {
                int _index = _list.IndexOf(_item) - 1;
                if (_index >= 0)
                {
                    return _list[_index];
                }
                else
                {
                    return default(T);
                }
            }
            return default(T);
        }
        public static Renderer ChangeMaterial(this Renderer target, Material _mat, int index)
        {
            Material[] _mats = target.materials;
            _mats[index] = _mat;
            target.materials = _mats;
            return target;
        }
    }

}
