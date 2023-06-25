// Automatically generated by Interoptopus.

#pragma warning disable 0105
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using com.github.AdaptiveHaptics;
#pragma warning restore 0105

namespace com.github.AdaptiveHaptics
{
    public static partial class AdapticsEngineInterop
    {
        public const string NativeLib = "adaptics_engine";

        static AdapticsEngineInterop()
        {
            var api_version = AdapticsEngineInterop.ffi_api_guard();
            if (api_version != 17028886466105721048ul)
            {
                throw new TypeLoadException($"API reports hash {api_version} which differs from hash in bindings (17028886466105721048). You probably forgot to update / copy either the bindings or the library.");
            }
        }


        /// use_mock_streaming: if true, use mock streaming. if false, use ulhaptics streaming
        /// enable_playback_updates: if true, enable playback updates, adaptics_engine_get_playback_updates expected to be called at 30hz.
        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "init_adaptics_engine")]
        public static extern IntPtr init_adaptics_engine(bool use_mock_streaming, bool enable_playback_updates);

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandleFFI` allocated by `init_adaptics_engine`
        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "deinit_adaptics_engine")]
        public static extern FFIError deinit_adaptics_engine(IntPtr handle, SliceMutu8 err_msg);

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandleFFI` allocated by `init_adaptics_engine`
        public static void deinit_adaptics_engine(IntPtr handle, byte[] err_msg)
        {
            var err_msg_pinned = GCHandle.Alloc(err_msg, GCHandleType.Pinned);
            var err_msg_slice = new SliceMutu8(err_msg_pinned, (ulong) err_msg.Length);
            try
            {
                var rval = deinit_adaptics_engine(handle, err_msg_slice);;
                if (rval != FFIError.Ok)
                {
                    throw new InteropException<FFIError>(rval);
                }
            }
            finally
            {
                err_msg_pinned.Free();
            }
        }

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandle` allocated by `init_adaptics_engine`
        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "adaptics_engine_update_pattern")]
        public static extern FFIError adaptics_engine_update_pattern(IntPtr handle, string pattern_json);

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandle` allocated by `init_adaptics_engine`
        public static void adaptics_engine_update_pattern_checked(IntPtr handle, string pattern_json)
        {
            var rval = adaptics_engine_update_pattern(handle, pattern_json);;
            if (rval != FFIError.Ok)
            {
                throw new InteropException<FFIError>(rval);
            }
        }

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandle` allocated by `init_adaptics_engine`
        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "adaptics_engine_update_playstart")]
        public static extern FFIError adaptics_engine_update_playstart(IntPtr handle, double playstart, double playstart_offset);

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandle` allocated by `init_adaptics_engine`
        public static void adaptics_engine_update_playstart_checked(IntPtr handle, double playstart, double playstart_offset)
        {
            var rval = adaptics_engine_update_playstart(handle, playstart, playstart_offset);;
            if (rval != FFIError.Ok)
            {
                throw new InteropException<FFIError>(rval);
            }
        }

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandle` allocated by `init_adaptics_engine`
        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "adaptics_engine_update_parameters")]
        public static extern FFIError adaptics_engine_update_parameters(IntPtr handle, string evaluator_params);

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandle` allocated by `init_adaptics_engine`
        public static void adaptics_engine_update_parameters_checked(IntPtr handle, string evaluator_params)
        {
            var rval = adaptics_engine_update_parameters(handle, evaluator_params);;
            if (rval != FFIError.Ok)
            {
                throw new InteropException<FFIError>(rval);
            }
        }

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandle` allocated by `init_adaptics_engine`
        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "adaptics_engine_reset_parameters")]
        public static extern FFIError adaptics_engine_reset_parameters(IntPtr handle);

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandle` allocated by `init_adaptics_engine`
        public static void adaptics_engine_reset_parameters_checked(IntPtr handle)
        {
            var rval = adaptics_engine_reset_parameters(handle);;
            if (rval != FFIError.Ok)
            {
                throw new InteropException<FFIError>(rval);
            }
        }

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandle` allocated by `init_adaptics_engine`
        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "adaptics_engine_update_user_parameters")]
        public static extern FFIError adaptics_engine_update_user_parameters(IntPtr handle, string user_parameters);

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandle` allocated by `init_adaptics_engine`
        public static void adaptics_engine_update_user_parameters_checked(IntPtr handle, string user_parameters)
        {
            var rval = adaptics_engine_update_user_parameters(handle, user_parameters);;
            if (rval != FFIError.Ok)
            {
                throw new InteropException<FFIError>(rval);
            }
        }

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandle` allocated by `init_adaptics_engine`
        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "adaptics_engine_update_geo_transform_matrix")]
        public static extern FFIError adaptics_engine_update_geo_transform_matrix(IntPtr handle, GeoMatrix geo_matrix);

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandle` allocated by `init_adaptics_engine`
        public static void adaptics_engine_update_geo_transform_matrix_checked(IntPtr handle, GeoMatrix geo_matrix)
        {
            var rval = adaptics_engine_update_geo_transform_matrix(handle, geo_matrix);;
            if (rval != FFIError.Ok)
            {
                throw new InteropException<FFIError>(rval);
            }
        }

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandle` allocated by `init_adaptics_engine`
        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "adaptics_engine_get_playback_updates")]
        public static extern FFIError adaptics_engine_get_playback_updates(IntPtr handle, ref SliceMutUnityEvalResult eval_results, out uint num_evals);

        /// # Safety
        /// `handle` must be a valid pointer to an `AdapticsEngineHandle` allocated by `init_adaptics_engine`
        public static void adaptics_engine_get_playback_updates(IntPtr handle, UnityEvalResult[] eval_results, out uint num_evals)
        {
            var eval_results_pinned = GCHandle.Alloc(eval_results, GCHandleType.Pinned);
            var eval_results_slice = new SliceMutUnityEvalResult(eval_results_pinned, (ulong) eval_results.Length);
            try
            {
                var rval = adaptics_engine_get_playback_updates(handle, ref eval_results_slice, out num_evals);;
                if (rval != FFIError.Ok)
                {
                    throw new InteropException<FFIError>(rval);
                }
            }
            finally
            {
                eval_results_pinned.Free();
            }
        }

        /// Guard function used by backends.
        ///
        /// Change impl version in this comment to force bump the API version.
        /// impl_version: 1
        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ffi_api_guard")]
        public static extern ulong ffi_api_guard();

    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct GeoMatrix
    {
        public double data0;
        public double data1;
        public double data2;
        public double data3;
        public double data4;
        public double data5;
        public double data6;
        public double data7;
        public double data8;
        public double data9;
        public double data10;
        public double data11;
        public double data12;
        public double data13;
        public double data14;
        public double data15;
    }

    /// !NOTE: y and z are swapped for Unity
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct UnityEvalCoords
    {
        public double x;
        public double y;
        public double z;
    }

    /// !NOTE: y and z are swapped for Unity
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct UnityEvalResult
    {
        /// !NOTE: y and z are swapped for Unity
        public UnityEvalCoords coords;
        public double intensity;
        public double pattern_time;
    }

    public enum FFIError
    {
        Ok = 0,
        NullPassed = 1,
        Panic = 2,
        OtherError = 3,
        AdapticsEngineThreadDisconnectedCheckDeinitForMoreInfo = 4,
        ErrMsgProvided = 5,
        EnablePlaybackUpdatesWasFalse = 6,
        ParameterJSONDeserializationFailed = 8,
    }

    ///A pointer to an array of data someone else owns which may be modified.
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct SliceMutUnityEvalResult
    {
        ///Pointer to start of mutable data.
        IntPtr data;
        ///Number of elements.
        ulong len;
    }

    public partial struct SliceMutUnityEvalResult : IEnumerable<UnityEvalResult>
    {
        public SliceMutUnityEvalResult(GCHandle handle, ulong count)
        {
            this.data = handle.AddrOfPinnedObject();
            this.len = count;
        }
        public SliceMutUnityEvalResult(IntPtr handle, ulong count)
        {
            this.data = handle;
            this.len = count;
        }
        public UnityEvalResult this[int i]
        {
            get
            {
                if (i >= Count) throw new IndexOutOfRangeException();
                var size = Marshal.SizeOf(typeof(UnityEvalResult));
                var ptr = new IntPtr(data.ToInt64() + i * size);
                return Marshal.PtrToStructure<UnityEvalResult>(ptr);
            }
            set
            {
                if (i >= Count) throw new IndexOutOfRangeException();
                var size = Marshal.SizeOf(typeof(UnityEvalResult));
                var ptr = new IntPtr(data.ToInt64() + i * size);
                Marshal.StructureToPtr<UnityEvalResult>(value, ptr, false);
            }
        }
        public UnityEvalResult[] Copied
        {
            get
            {
                var rval = new UnityEvalResult[len];
                for (var i = 0; i < (int) len; i++) {
                    rval[i] = this[i];
                }
                return rval;
            }
        }
        public int Count => (int) len;
        public IEnumerator<UnityEvalResult> GetEnumerator()
        {
            for (var i = 0; i < (int)len; ++i)
            {
                yield return this[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }


    ///A pointer to an array of data someone else owns which may be modified.
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct SliceMutu8
    {
        ///Pointer to start of mutable data.
        IntPtr data;
        ///Number of elements.
        ulong len;
    }

    public partial struct SliceMutu8 : IEnumerable<byte>
    {
        public SliceMutu8(GCHandle handle, ulong count)
        {
            this.data = handle.AddrOfPinnedObject();
            this.len = count;
        }
        public SliceMutu8(IntPtr handle, ulong count)
        {
            this.data = handle;
            this.len = count;
        }
        public byte this[int i]
        {
            get
            {
                if (i >= Count) throw new IndexOutOfRangeException();
                var size = Marshal.SizeOf(typeof(byte));
                var ptr = new IntPtr(data.ToInt64() + i * size);
                return Marshal.PtrToStructure<byte>(ptr);
            }
            set
            {
                if (i >= Count) throw new IndexOutOfRangeException();
                var size = Marshal.SizeOf(typeof(byte));
                var ptr = new IntPtr(data.ToInt64() + i * size);
                Marshal.StructureToPtr<byte>(value, ptr, false);
            }
        }
        public byte[] Copied
        {
            get
            {
                var rval = new byte[len];
                for (var i = 0; i < (int) len; i++) {
                    rval[i] = this[i];
                }
                return rval;
            }
        }
        public int Count => (int) len;
        public IEnumerator<byte> GetEnumerator()
        {
            for (var i = 0; i < (int)len; ++i)
            {
                yield return this[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }




    public class InteropException<T> : Exception
    {
        public T Error { get; private set; }

        public InteropException(T error): base($"Something went wrong: {error}")
        {
            Error = error;
        }
    }

}
